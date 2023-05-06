using Microsoft.AspNetCore.Mvc;
using Share;
using System.Text;
using System.Text.RegularExpressions;

namespace clotest.Controllers
{
    [Route("[controller]")]
    public class disksController : Controller
    {

        /// <summary>
        /// 현재 프로젝트가 구성되고 있는 Machine의 Disk List 반환하는 API
        /// uid는 편의상 드라이브 네임의 영문만 return
        /// diskSize는 편의상 GB로 변환 및 소수점 2자리까지만 표시
        /// API 식별코드 : API1
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get()
        {

            try 
            {
                                                
                //API 호출시 식별코드를 담아 log 기록
                Library.LogWrite(Request, "API1");

                string currentPath = Directory.GetCurrentDirectory();

                //현재 경로의 드라이브 정보 가져오기
                DriveInfo currentDrive = new DriveInfo(Path.GetPathRoot(currentPath));

                string drive_char = Regex.Replace(currentDrive.Name, @"[^a-zA-Z]", "");                                     //디스크 네임 
                double total_size = Math.Round((double)currentDrive.TotalSize / Math.Pow(1024, 3), 2);                //디스크 전체 사이즈 

                var driveInfo = new
                {
                    uid = drive_char,
                    size = total_size
                };

                return Json(driveInfo);
                
            }
            catch (Exception ex)
            {
                return NotFound("에러가 발생 했습니다." + ex.Message);
            }

        }

        /// <summary>
        /// 특정  disk의 directory structure 반환하는 API
        /// API 식별코드 : API2
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpGet("{uid}")]
        public ActionResult Get(string uid)
        {
            try
            {
                //API 호출시 식별코드를 담아 log 기록
                Library.LogWrite(Request, "API2");

                string root_path = uid + @":\";

                // 드라이브 경로가 존재하는지 여부 확인
                if (!Directory.Exists(root_path))
                {
                    return NotFound("해당 드라이브는 존재 하지 않습니다. : " + uid);
                }

                //uid 드라이브의 디렉토리 정보 
                DirectoryInfo directory = new DirectoryInfo(root_path);

                // 디렉토리 구조를 저장할 리스트 생성
                List<directoryModel> directoryList = new List<directoryModel>();

                // uid drive 하위 디렉토리 구조 추가 
                directoryList.AddRange(Library.GetDirectoryStructure(root_path ,1));

                return Json(directoryList);

            }
            catch(Exception ex)
            {
                return NotFound("에러가 발생 했습니다." + ex.Message);
            }
        }

        /// <summary>
        /// 특정위치에 folder/file 생성하는 API
        /// 전달받은 드라이브에 main 폴더(CLO) 생성
        /// main 폴더 밑에 서브 폴더(CLO_SUB) 생성 및 임의 파일 생성(.txt)
        /// 서브폴더가 존재하면 폴더명 + _숫자로 서브 폴더 계속 생성
        /// API 식별코드 : API3
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> PostAsync(string uid)
        {
            try 
            {
                //API 호출시 식별코드를 담아 log 기록
                Library.LogWrite(Request, "API3");

                string root_path = uid + @":\";
                string main_folderName = "CLO";
                string sub_folderName = "CLO_SUB";
                string newFolderName = sub_folderName;

                // 드라이브 경로가 존재하는지 여부 확인
                if (!Directory.Exists(root_path))
                {
                    return NotFound("해당 드라이브는 존재 하지 않습니다. : " + uid);
                }

                // 상위(CLO) 폴더 경로 생성
                string main_folderPath = Path.Combine(root_path, main_folderName);

                // 상위 폴더가 존재하지 않을 경우 새로 생성
                if (!Directory.Exists(main_folderPath))
                {
                    Directory.CreateDirectory(main_folderPath);
                }

                // 하위 폴더 경로 생성
                string sub_folderPath = Path.Combine(main_folderPath, sub_folderName);

                if (!Directory.Exists(sub_folderPath))
                {
                    Directory.CreateDirectory(sub_folderPath);
                }
                else
                {
                    // 동일한 이름의 폴더가 존재할 경우 다른 이름으로 생성
                    int count = 1;

                    while (Directory.Exists(sub_folderPath))
                    {
                        newFolderName = sub_folderName + "_" + count.ToString();
                        sub_folderPath = Path.Combine(main_folderPath, newFolderName);
                        count++;
                    }
                    Directory.CreateDirectory(sub_folderPath);
                }

                // 텍스트 파일 경로 생성
                string fileName = newFolderName + ".txt";
                string filePath = Path.Combine(sub_folderPath, fileName);

                // 텍스트 파일 생성
                using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    await sw.WriteAsync("CLO BEST!");
                }

                var directoryInfo = new
                {
                    path = sub_folderPath,
                    content = fileName
                };

                return Json(directoryInfo);
            } 
            catch (Exception ex) 
            {
                return NotFound("에러가 발생 했습니다." + ex.Message);
            }
            
        }
    }
}

