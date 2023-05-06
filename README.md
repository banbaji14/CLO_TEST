# 프로젝트명

클로버추어패션 과제 전형 - Backend Jeju 프로젝트(배성환)

# 프로젝트 실행

clone-repository 의 clotest/clotest.sln 실행

# 개발환경

개발 및 소스코드는 windows 환경 기준으로 작성

# 선택사항(Libraries / Packages)

 - Swashbuckle
 - Logging
   - log4net

 # 기타 참고 사항
 
 - windows c:\에서 디렉토리 구조 반환 시 서버에서 전달한 모든 데이터를 클라이언트 측에서 전체 수시 하지 못하고 연결을 끊어 오류 발생 가능성 있음
 - 폴더 접근 시 windows api를 사용하거나 기타 방법들로 권하 변경하여 접근하는 방법도 있지마 이번 과제에서 예외처리로 접근 권한이 없는 폴더는 제외하고 반환
 - 특정 위치에 폴더/파일 생성 시 특정 드라이브에 CLO라는 메이 폴더를 만들고 API를 호출 할 때마다 하위 폴더 및 파일을 계속해서 생성하는 방식으로 구현
