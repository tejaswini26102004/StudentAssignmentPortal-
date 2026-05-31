# StudentAssignmentPortal-
Full stack assignment portal using ASP.NET Core, ADO.NET, AngularJS

🏗️ Project 1 — Student Assignment Portal (ADO.NET + AngularJS)
▶️ How to Run the App
Step 1 — Start the Backend

Open Visual Studio
Open project → File → Recent Projects → AssignmentPortal
Press F5
Wait for console to show:

Now listening on: http://localhost:5235
Step 2 — Start the Frontend

Open Command Prompt
Type:

cd C:\Users\tejaswini.sivasamy\Downloads\StudentAssignmentPortal-\frontend

Type:

npx http-server . -p 8080 --cors
Step 3 — Open the App
Open browser and go to:
http://localhost:8080

🧪 How to Run the Tests
Backend Tests (xUnit + Moq):

Backend does NOT need to be running
Open Visual Studio
Click Test menu → Run All Tests
✅ 12 tests should pass

Frontend Tests (Jasmine + Karma):

Backend does NOT need to be running
Open Command Prompt
Type:

cd C:\Users\tejaswini.sivasamy\Downloads\StudentAssignmentPortal-\frontend

Type:

npm test

✅ 15 tests should pass


🛑 How to Stop
WhatHow to StopBackendPress Shift+F5 in Visual Studio or close console windowFrontendGo to Command Prompt → press Ctrl+C
