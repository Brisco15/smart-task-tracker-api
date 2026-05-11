# 🚀 Smart Task Tracker

A full-stack task management application built with Angular and .NET 9, deployed on Microsoft Azure.

## 📋 Table of Contents

- [Features](#features)
- [Tech Stack](#tech-stack)
- [Architecture](#architecture)
- [Live Demo](#live-demo)
- [Prerequisites](#prerequisites)
- [Local Development Setup](#local-development-setup)
- [Deployment](#deployment)
- [API Documentation](#api-documentation)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)

---

## ✨ Features

- **User Authentication & Authorization**
  - JWT-based authentication
  - Role-based access control (Admin, Manager, Developer)
  - Secure password hashing

- **Project Management**
  - Create, read, update, and delete projects
  - Assign projects to teams
  - Track project status

- **Task Management**
  - Create and assign tasks to users
  - Set priorities and deadlines
  - Update task status (To Do, In Progress, Done)
  - Task filtering and sorting

- **User Management**
  - User registration and login
  - Role assignment
  - User profiles

- **Real-time Updates**
  - Responsive UI with instant feedback
  - Angular reactive forms

---

## 🛠️ Tech Stack

### Frontend
- **Framework**: Angular 19
- **Language**: TypeScript
- **Styling**: CSS3
- **HTTP Client**: Angular HttpClient
- **Routing**: Angular Router
- **Forms**: Reactive Forms

### Backend
- **Framework**: ASP.NET Core 9.0
- **Language**: C# 12
- **ORM**: Entity Framework Core 9.0
- **Database**: PostgreSQL 16
- **Authentication**: JWT (JSON Web Tokens)
- **API Documentation**: Swagger/OpenAPI

### Cloud Infrastructure (Azure)
- **Frontend Hosting**: Azure Static Web Apps (Free Tier)
- **Backend Hosting**: Azure App Service (Free F1 Tier)
- **Database**: Azure Database for PostgreSQL (Burstable B1ms)
- **Region**: West Europe

---

## 🏗️ Architecture

The Smart Task Tracker application follows a client-server architecture:

- **Client**: Angular frontend (smart-task-tracker-ui) - Consumes the backend API, handles user interactions, and displays data.
- **Server**: ASP.NET Core Web API (smartTaskTracker.API) - Provides RESTful API endpoints for data access and management.
- **Database**: PostgreSQL - Stores application data, including user information, projects, tasks, and time tracking.
- **Cloud Services**: Hosted on Microsoft Azure, utilizing services like Azure App Service, Azure Static Web Apps, and Azure Database for PostgreSQL.

### Component Diagram

```plaintext
[Angular Frontend] <---> [ASP.NET Core Web API] <---> [PostgreSQL Database]
        |                           |                            |
        |                           |                            |
 [Azure Static Web Apps]    [Azure App Service]    [Azure Database for PostgreSQL]
```

---

## 🚀 Live Demo


---

## 🌐 Live Demo

- **Frontend**: [https://calm-ocean-0dd1c2e03.7.azurestaticapps.net](https://calm-ocean-0dd1c2e03.7.azurestaticapps.net)
- **Backend API**: [https://smart-task-tracker-api-4114.azurewebsites.net](https://smart-task-tracker-api-4114.azurewebsites.net)
- **Swagger Documentation**: [https://smart-task-tracker-api-4114.azurewebsites.net/swagger](https://smart-task-tracker-api-4114.azurewebsites.net/swagger)

### Test Credentials

---

## 📋 Prerequisites

### For Local Development

- **Node.js**: v18.x or higher
- **npm**: v9.x or higher
- **.NET SDK**: 9.0 or higher
- **PostgreSQL**: 16.x
- **Git**: Latest version
- **Angular CLI**: `npm install -g @angular/cli`
- **EF Core Tools**: `dotnet tool install --global dotnet-ef`

### For Azure Deployment

- **Azure CLI**: [Installation Guide](https://learn.microsoft.com/cli/azure/install-azure-cli)
- **Azure Account**: [Free Account](https://azure.microsoft.com/free/)
- **Azure Static Web Apps CLI**: `npm install -g @azure/static-web-apps-cli`


---

## 🛠️ Installation

### 1. Clone Repository

### 2. Backend Setup (smartTaskTracker.API)

#### 2.1 Verify .NET 10 Installation

#### 2.2 Install Dependencies
```bash
cd smartTaskTracker.API
dot asdfasdfas
```

#### 2.3 Configure Database

The API uses SQL Server LocalDB. The connection string is already configured in `appsettings.json`:

The `appsettings.json` already contains:
#### 2.4 Configure JWT Secret

For development, we use User Secrets:

#### 2.5 Create and Migrate Database

```bash
dotnet ef database update
```

If migrations are missing:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

#### 2.6 Start API

```bash
dotnet run
```

The API runs on:
- HTTPS: `https://localhost:5261`
- HTTP: `http://localhost:5260`
- Swagger: `http://localhost:5260/swagger`

### 3. Frontend Setup (smart-task-tracker-ui)

#### 3.1 Install Dependencies
```bash
cd smart-task-tracker-ui
npm install
```

#### 3.2 Verify API URL

The API URL is already configured in the services (`http://localhost:5260/api`). If the API runs on a different port, adjust the URLs in the service files:

- `src/app/services/auth.ts`
- `src/app/services/project-service.ts`
- `src/app/services/task-service.ts`
- `src/app/services/time-tracking.ts`
- `src/app/services/admin.ts`

#### 3.3 Start Development Server
```bash
ng serve
```

---

## 🎓 Usage

- Access the frontend at `http://localhost:4200`
- API documentation available at `http://localhost:5000/swagger`
- Angular frontend communicates with the backend API for data operations

---

## ⚙️ Development

- Follow standard RESTful API practices for backend development
- For frontend, use Angular style guide and best practices
- Commit code to respective branches and create pull requests for review

## 📝 Development

### Build Backend

### Run Backend Tests

---

## 📝 Contributing

- Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details.

---

## 📄 License

This project is private and intended for personal use.

---

## 📞 Support

For support, open an issue on GitHub or contact the project maintainer.

---

## Acknowledgments
- Inspired by various task management tools and frameworks
- Built with passion and dedication to improve productivity

Enjoy tracking your tasks smartly!

The frontend runs on: `http://localhost:4200`

---

## 🔐 Authentication & Authorization

### User Roles

| Role | Permissions |
|------|-------------|
| **Admin** | User management, full access to system settings |
| **Manager** | Project and task management, team management |
| **Developer** | Access to assigned tasks, time tracking |

### Default Test Users

### JWT Token Flow

1. **Login**: `POST /api/auth/login` with email & password
2. **Receive Token**: JWT token in response
3. **Store Token**: Frontend stores token in `localStorage`
4. **Authenticate Requests**: Token is sent in `Authorization` header
5. **Automatic Renewal**: Token expires after 24 hours

---

## 📚 API Endpoints

### Authentication
POST /api/auth/register - Register new user 
POST /api/auth/login - Login user (receive JWT)

### Projects
GET    /api/projects - Get all projects GET    /api/projects/{id}- Get project by ID 
POST   /api/projects - Create new project 
(Manager) PUT    /api/projects/{id} - Update project 
(Manager) DELETE /api/projects/{id} - Delete project (Manager) 
PATCH  /api/projects/{id}/archive - Archive project (Manager)

### Tasks
GET    /api/tasks - Get all tasks 
GET    /api/tasks/{id} - Get task by ID 
GET    /api/tasks/project/{projectId}  - Get tasks by project
POST   /api/tasks - Create new task (Manager) 
PUT    /api/tasks/{id}- Update task 
DELETE /api/tasks/{id}- Delete task (Manager) 
PATCH  /api/tasks/{id}/archive - Archive task

### Time Tracking
GET    /api/timetrackings/project/{projectId}- Time per project
GET    /api/timetrackings/project/{projectId}/total- Total time per project
GET    /api/timetrackings/task/{taskId}- Time per task 
POST   /api/timetrackings/start/{taskId}- Start time tracking 
POST   /api/timetrackings/stop/{taskId} - Stop time tracking

### User Management (Admin)
GET    /api/users              - Get all users (Admin)
GET    /api/users/{id}         - Get user by ID (Admin)
PUT    /api/users/{id}         - Update user (Admin) 
DELETE /api/users/{id}         - Delete user (Admin) 
PATCH  /api/users/{id}/archive - Archive user (Admin)

Full API documentation: `http://localhost:5260/swagger`

---

## 🗄️ Database Structure

### Main Tables

- **Users**: Users with roles and authentication
- **Roles**: Admin, Manager, Developer
- **Projects**: Projects with timestamps and archiving
- **TaskItems**: Tasks with status, priority, and assignment
- **TaskStatuses**: Pending, In Progress, Completed
- **TaskPriorities**: Low, Medium, High
- **TimeTrackings**: Time entries with start, end, and duration

### Entity Relationships

### Entity Framework Core 10 Features Used

- **Minimal APIs**: Lightweight endpoint configuration
- **Global Query Filters**: Automatic filtering of archived items
- **Shadow Properties**: Audit fields (CreatedAt, UpdatedAt)
- **Value Converters**: Custom type mapping
- **Interceptors**: Automatic soft delete implementation

---

## 🌐 CORS Configuration

The API is configured for the Angular frontend:

**Important**: Adjust the origin URL for production!

---

## 🎨 Frontend Structure





### Build Frontend

Production build is placed in `dist/smart-task-tracker-ui/`

### Code Style

- **Backend**: C# Coding Conventions (Microsoft Style)
- **Frontend**: Angular Style Guide + TypeScript Best Practices
- **Formatting**: Prettier (Frontend), EditorConfig (Backend)

## 🐛 Troubleshooting

### Backend Issues

**Problem**: `Cannot open database "SmartTaskTrackerDB"`

## 🚀 Deployment

### Backend Deployment

1. **Update Connection String** for production database
2. **Configure Azure Key Vault** for secrets (production)
3. **Publish to Azure App Service** or IIS
4. **Update CORS policy** with production frontend URL

### Frontend Deployment

1. **Build for production**
2. **Update API URL** in environment files
3. **Deploy to static hosting** (Azure Static Web Apps, Netlify, Vercel)

---

## 🔒 Security Best Practices

- ✅ JWT tokens stored in `localStorage` (consider `httpOnly` cookies for production)
- ✅ Password hashing with BCrypt
- ✅ Role-based authorization on all protected endpoints
- ✅ HTTPS enforced in production
- ✅ SQL injection protection via EF Core parameterized queries
- ✅ CORS restricted to specific origins
- ✅ User Secrets for development
- ✅ Azure Key Vault for production secrets
- ⚠️ **TODO**: Implement refresh tokens for better security
- ⚠️ **TODO**: Add rate limiting to prevent abuse

---

## 🧪 Testing

### Backend Tests

### Frontend Tests

### Integration Tests

---

## 🔗 Related Links

- [.NET 9 Documentation](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-9)
- [Angular Documentation](https://angular.io/docs)
- [Entity Framework Core 9](https://learn.microsoft.com/ef/core/)
- [JWT.io](https://jwt.io/) - JWT Token Debugger
- [Angular Material](https://material.angular.io/)
- [Chart.js](https://www.chartjs.org/)
- [Azure Key Vault](https://learn.microsoft.com/azure/key-vault/)

---

## 📄 License

This project is private and intended for personal use.

---

## 👤 Author

**Brisco**
- GitHub: [@Brisco15](https://github.com/Brisco15)

---

## 🤝 Contributing

As this is a private project, external contributions are not currently accepted.

---

## 📊 Project Status

- ✅ Backend API with JWT authentication (.NET 9)
- ✅ Frontend with Angular Material
- ✅ Dashboard with statistics and charts
- ✅ Time tracking per task
- ✅ Role-based access control
- ✅ Project and task management
- ✅ Admin panel for user management
- ✅ Entity Framework Core 9
- ✅ Azure Key Vault integration
- 🔄 Refresh token implementation (planned)
- 🔄 Unit tests (in progress)
- 🔄 E2E tests (planned)

---

## 📈 Future Enhancements

- [ ] Email notifications for task assignments
- [ ] Real-time updates with SignalR
- [ ] Advanced reporting and analytics
- [ ] File attachments for tasks
- [ ] Task comments and activity log
- [ ] Dark mode support
- [ ] Multi-language support (i18n)
- [ ] Export to Excel/PDF
- [ ] Gantt chart view for projects

---


**Version**: 1.0.0  
**Last Updated**: May 2026  
**.NET Version**: 9.0  
**Angular Version**: 18+

## 🐛 Known Issues

- First load may be slow due to cold start (free tier)
- Limited to 60 minutes/day compute time (free App Service)


## 🙏 Acknowledgments

- Microsoft Azure for cloud hosting
- Angular team for the amazing framework
- ASP.NET Core team for the robust backend framework
- PostgreSQL for the reliable database