# 🎓 Full Stack Web Applications

Two full-stack web applications built as part of a Windows Web Application assignment using ASP.NET Core Web API, SQL Server, and modern frontend frameworks.

---

## 📁 Projects Overview

| | Project 1 | Project 2 |
|--|-----------|-----------|
| **Name** | Student Assignment Portal | Online Quiz Management System |
| **Backend** | ASP.NET Core Web API + ADO.NET | ASP.NET Core Web API + Entity Framework Core |
| **Frontend** | AngularJS | React |
| **Database** | SQL Server (Manual SQL) | SQL Server (Code First Migrations) |
| **Auth** | JWT + Role-based | JWT + Role-based |
| **Backend Tests** | xUnit + Moq (12 tests) | xUnit + Moq (15 tests) |
| **Frontend Tests** | Jasmine + Karma (15 tests) | Jest (20 tests) |
| **E2E Tests** | None | Selenium (12 tests) |

---

## 👤 Test Accounts

| Role | Email | Password |
|------|-------|----------|
| Teacher | teacher@test.com | Test@123 |
| Student | student@test.com | Test@123 |

---

---

# 📘 Project 1 — Student Assignment Portal

## 🔧 Tech Stack
- **Backend:** ASP.NET Core Web API, ADO.NET, SQL Server
- **Frontend:** AngularJS
- **Auth:** JWT Tokens + Role-based Authorization
- **Testing:** xUnit + Moq (Backend), Jasmine + Karma (Frontend)

## 👥 Roles
| Role | Permissions |
|------|------------|
| Teacher | Create, Edit, Delete Assignments + View Submissions |
| Student | View Assignments + Submit Assignments |

## 🗄️ Database Tables
- **Users** — Stores Teachers and Students
- **Assignments** — Created by Teachers
- **Submissions** — Submitted by Students

## 🔗 API Endpoints
| Method | Endpoint | Access |
|--------|----------|--------|
| POST | /api/auth/register | Public |
| POST | /api/auth/login | Public |
| GET | /api/assignments | Both |
| POST | /api/assignments | Teacher only |
| PUT | /api/assignments/{id} | Teacher only |
| DELETE | /api/assignments/{id} | Teacher only |
| POST | /api/assignments/submit | Student only |
| GET | /api/assignments/{id}/submissions | Teacher only |

## ▶️ How to Run the App

### Step 1 — Start the Backend
1. Open **Visual Studio**
2. Open project → **File → Recent Projects → AssignmentPortal**
3. Press **F5**
4. Wait for the console to show:
```
Now listening on: http://localhost:5235
```

### Step 2 — Start the Frontend
1. Open **Command Prompt**
2. Navigate to the frontend folder:
```
cd StudentAssignmentPortal-\frontend
```
3. Start the local server:
```
npx http-server . -p 8080 --cors
```

### Step 3 — Open the App
Open your browser and go to:
```
http://localhost:8080
```

## 🧪 How to Run the Tests

### Backend Tests (xUnit + Moq)
> ⚠️ Backend does NOT need to be running

- Open **Visual Studio**
- Click **Test** menu → **Run All Tests**
- ✅ 12 tests should pass

### Frontend Tests (Jasmine + Karma)
> ⚠️ Backend does NOT need to be running

Open **Command Prompt** and run:
```
cd StudentAssignmentPortal-\frontend
npm test
```
- ✅ 15 tests should pass

## 🛑 How to Stop
| What | How to Stop |
|------|------------|
| Backend | Press **Shift+F5** in Visual Studio |
| Frontend | Go to Command Prompt → press **Ctrl+C** |

---

---



