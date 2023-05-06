using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Share;


namespace clotest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class callController : Controller
    {
        /// <summary>
        /// API별 현재까지 요청 수 반환하는 API
        /// 각 API 호출 시 기록한 API코드를 로그 파일에서 정규식을 이용해 찾아 카운트
        /// API 식별코드 : API4
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Get() 
        {

            //API 호출시 식별코드를 담아 log 기록
            Library.LogWrite(Request, "API4");

            int api1_count = 0, api2_count = 0, api3_count = 0, api4_count = 0;

            //로그 폴더를 가져오고 파일 경로를 셋팅
            string logPath = Library.GetLogFolderPath();
            string filePath = Path.Combine(logPath, "App.log");
            string[] files = Directory.GetFiles(logPath, "App.log");
            
            // 로그 경로가 존재하는지 여부 확인
            if (!Directory.Exists(logPath))
            {
                return NotFound("로그경로가 존재 하지 않습니다. : " + logPath);
            }

            // 해당경로에 파일이 존재하는지 확인
            if (files.Length < 0)
            {
                return NotFound("해당 경로에 파일이 없습니다. - " + filePath);
            }

            //파일을 open하고 한줄씩 정규식을 이용해 식별코드를 확인
            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var streamReader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var api1_match = Regex.Match(line, "API1");
                        if (api1_match.Success)
                        {
                            api1_count++;
                        }

                        var api2_match = Regex.Match(line, "API2");
                        if (api2_match.Success)
                        {
                            api2_count++;
                        }

                        var api3_match = Regex.Match(line, "API3");
                        if (api3_match.Success)
                        {
                            api3_count++;
                        }

                        var api4_match = Regex.Match(line, "API4");
                        if (api4_match.Success)
                        {
                            api4_count++;
                        }
                    }
                }
            }
            
            //리스트 생성 후 결과값 추가
            List<callModel> callModels = new List<callModel>();

            callModels.Add(new callModel { Path = "API1 : diskList_API", Count = api1_count });
            callModels.Add(new callModel { Path = "API2 : disk_directoryStructure_API", Count = api2_count });
            callModels.Add(new callModel { Path = "API3 : folder/file_API", Count = api3_count });
            callModels.Add(new callModel { Path = "API4 : apiCallCount_API", Count = api4_count });

            return Json(callModels);

        }
    }
}
