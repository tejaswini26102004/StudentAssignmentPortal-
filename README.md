📘 Project 1 — Student Assignment Portal
🔧 Tech Stack

Backend: ASP.NET Core Web API, ADO.NET, SQL Server
Frontend: AngularJS
Auth: JWT Tokens + Role-based Authorization
Testing: xUnit + Moq (Backend), Jasmine + Karma (Frontend)

👥 Roles
RolePermissionsTeacherCreate, Edit, Delete Assignments + View SubmissionsStudentView Assignments + Submit Assignments
🗄️ Database Tables

Users — Stores Teachers and Students
Assignments — Created by Teachers
Submissions — Submitted by Students

🔗 API Endpoints
MethodEndpointAccessPOST/api/auth/registerPublicPOST/api/auth/loginPublicGET/api/assignmentsBothPOST/api/assignmentsTeacher onlyPUT/api/assignments/{id}Teacher onlyDELETE/api/assignments/{id}Teacher onlyPOST/api/assignments/submitStudent onlyGET/api/assignments/{id}/submissionsTeacher only
▶️ How to Run the App
Step 1 — Start the Backend

Open Visual Studio
Open project → File → Recent Projects → AssignmentPortal
Press F5
Wait for the console to show:

Now listening on: http://localhost:5235
Step 2 — Start the Frontend

Open Command Prompt
Navigate to the frontend folder:

cd StudentAssignmentPortal-\frontend

Start the local server:

npx http-server . -p 8080 --cors
Step 3 — Open the App
Open your browser and go to:
http://localhost:8080
🧪 How to Run the Tests
Backend Tests (xUnit + Moq)

⚠️ Backend does NOT need to be running


Open Visual Studio
Click Test menu → Run All Tests
✅ 12 tests should pass

Frontend Tests (Jasmine + Karma)

⚠️ Backend does NOT need to be running

Open Command Prompt and run:
cd StudentAssignmentPortal-\frontend
npm test

✅ 15 tests should pass

🛑 How to Stop
WhatHow to StopBackendPress Shift+F5 in Visual StudioFrontendGo to Command Prompt → press Ctrl+C
