using Share;
using log4net.Repository;
using log4net;
using System.Web;

namespace clotest
{

    public class Library
    {
       /// <summary>
       /// 재귀 함수 
       /// 현재 디렉토리의 하위디렉토리를 가져오는 함수
       /// maxDepth는 32로 설정
       /// 폴더 권한문제는 windows api를 사용해서 권한에서 변경하는 방법도 존재 하지만 예외처리를 통해 접근 가능한 폴더만 나열하도록 구성
       /// </summary>
       /// <param name="directoryPath"></param>
       /// <param name="depth"></param>
       /// <returns></returns>
        public static List<directoryModel> GetDirectoryStructure(string directoryPath, int depth)
        {
            //재귀 함수 사용 시 디렉토리 depth 제한 설정
            int maxDepth = 32;
            
            List<directoryModel> directories = new List<directoryModel>();
            
            if (maxDepth < depth)
            {
                return directories;
            }

            // 현재 디렉토리의 서브디렉토리 리스트 구하기
            string[] subdirectories = Directory.GetDirectories(directoryPath);

            foreach (string subdirectory in subdirectories)
            {
                try
                {
                    // 현재 디렉토리명, 경로 추가
                    directoryModel directoryModel = new directoryModel
                    {
                        Name = Path.GetFileName(subdirectory),
                        Path = subdirectory,
                        Children = new List<directoryModel>()
                    };

                    // 현재 디렉토리의 하위 디렉토리를 재귀호출하여 검색
                    directoryModel.Children.AddRange(GetDirectoryStructure(subdirectory, depth + 1));

                    directories.Add(directoryModel);
                }
                catch (UnauthorizedAccessException uae)
                {
                    ILog log = LogManager.GetLogger("");
                    log.Info(uae.Message);
                }
            }

            return directories;
        }

        /// <summary>
        /// 로그를 기록하는 로그폴더를 구하는 함수
        /// </summary>
        /// <returns></returns>
        public static string GetLogFolderPath()
        {
            ILoggerRepository repository = log4net.LogManager.GetRepository();
            var appenders = repository.GetAppenders();
            foreach (var appender in appenders)
            {
                var fileAppender = appender as log4net.Appender.FileAppender;
                if (fileAppender != null)
                {
                    return Path.GetDirectoryName(fileAppender.File);
                }
            }
            return null;
        }

        /// <summary>
        ///  로그를 기록하는 함수 
        ///  api 식별코드, 요청방식, api_uri를 기록
        /// </summary>
        /// <param name="Request"></param>
        /// <param name="api_id"></param>
        public static void LogWrite(HttpRequest Request , string api_id) 
        {
            ILog log = LogManager.GetLogger("");

            // 호출한 API URL 가져오기
            var api_url = $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            
            string decodedQueryString = HttpUtility.UrlDecode(api_url);

            log.Info($"api_id: {api_id}, Request.Method: {Request.Method}, Call_API: {decodedQueryString}");

        }
    }
}

