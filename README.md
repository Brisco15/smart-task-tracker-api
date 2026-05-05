# Smart Task Tracker

A complete task management system with .NET 10 backend and Angular frontend for project management, time tracking, and role-based access control.

## 📋 Overview

The Smart Task Tracker system consists of two main components:
- **Backend**: `smartTaskTracker.API` - ASP.NET Core Web API with JWT authentication (.NET 10)
- **Frontend**: `smart-task-tracker-ui` - Angular Standalone application with Material Design

## 🚀 Features

### Backend (smartTaskTracker.API)
- **JWT Authentication**: Secure token-based authentication
- **Role-Based Authorization**: Admin, Manager, and Developer roles
- **Project Management**: CRUD operations for projects with archiving
- **Task Management**: Complete task management with status and priorities
- **Time Tracking**: Start/stop timer per task with duration calculation
- **Entity Framework Core 10**: SQL Server LocalDB integration
- **CORS Support**: Configured for Angular frontend
- **Swagger/OpenAPI**: Interactive API documentation
- **Soft Delete**: Safe deletion without permanent data loss
- **Azure Key Vault Integration**: Secure secret management for production

### Frontend (smart-task-tracker-ui)
- **Dashboard**: Overview with statistics, charts, and time tracking
- **Project Management**: Project CRUD with archiving
- **Task Management**: Task board with filtering and time tracking
- **User Management**: Admin panel for user management
- **Angular Material**: Modern UI components
- **Chart.js**: Interactive visualizations
- **Responsive Design**: Mobile-friendly layout

## 📋 Prerequisites

### Backend
- [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [SQL Server LocalDB](https://learn.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) (installed with Visual Studio)
- [Visual Studio 2022+](https://visualstudio.microsoft.com/) or [Visual Studio Code](https://code.visualstudio.com/)

### Frontend
- [Node.js](https://nodejs.org/) (v18 or higher)
- [Angular CLI](https://angular.io/cli) (v18+)
- Modern web browser (Chrome, Firefox, Edge)

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

## 🎓 Usage

- Access the frontend at `http://localhost:4200`
- API documentation available at `http://localhost:5000/swagger`
- Angular frontend communicates with the backend API for data operations

## ⚙️ Development

- Follow standard RESTful API practices for backend development
- For frontend, use Angular style guide and best practices
- Commit code to respective branches and create pull requests for review

## 📝 Development

### Build Backend

### Run Backend Tests

## 📝 Contributing

- Contributions are welcome! Please read [CONTRIBUTING.md](CONTRIBUTING.md) for details.

## 📄 License

This project is private and intended for personal use.

## 📞 Support

For support, open an issue on GitHub or contact the project maintainer.

## Acknowledgments
- Inspired by various task management tools and frameworks
- Built with passion and dedication to improve productivity

Enjoy tracking your tasks smartly!

The frontend runs on: `http://localhost:4200`

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

## 🌐 CORS Configuration

The API is configured for the Angular frontend:

**Important**: Adjust the origin URL for production!

## 🎨 Frontend Structure

## 📦 NuGet Packages (.NET 10)



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

## 🧪 Testing

### Backend Tests

### Frontend Tests

### Integration Tests

## 🔗 Related Links

- [.NET 10 Documentation](https://learn.microsoft.com/dotnet/core/whats-new/dotnet-10)
- [Angular Documentation](https://angular.io/docs)
- [Entity Framework Core 10](https://learn.microsoft.com/ef/core/)
- [JWT.io](https://jwt.io/) - JWT Token Debugger
- [Angular Material](https://material.angular.io/)
- [Chart.js](https://www.chartjs.org/)
- [Azure Key Vault](https://learn.microsoft.com/azure/key-vault/)

## 📄 License

This project is private and intended for personal use.

## 👤 Author

**Brisco**
- GitHub: [@Brisco15](https://github.com/Brisco15)

## 🤝 Contributing

As this is a private project, external contributions are not currently accepted.

## 📊 Project Status

- ✅ Backend API with JWT authentication (.NET 10)
- ✅ Frontend with Angular Material
- ✅ Dashboard with statistics and charts
- ✅ Time tracking per task
- ✅ Role-based access control
- ✅ Project and task management
- ✅ Admin panel for user management
- ✅ Entity Framework Core 10
- ✅ Azure Key Vault integration
- 🔄 Refresh token implementation (planned)
- 🔄 Unit tests (in progress)
- 🔄 E2E tests (planned)

## 📈 Future Enhancements

- [ ] Email notifications for task assignments
- [ ] Real-time updates with SignalR
- [ ] Advanced reporting and analytics
- [ ] File attachments for tasks
- [ ] Task comments and activity log
- [ ] Mobile app (React Native/Flutter)
- [ ] Dark mode support
- [ ] Multi-language support (i18n)
- [ ] Export to Excel/PDF
- [ ] Gantt chart view for projects

## 🆕 What's New in .NET 10

This project leverages the latest .NET 10 features:
- **Performance Improvements**: Faster startup and runtime
- **Enhanced Minimal APIs**: Simplified endpoint configuration
- **Improved EF Core**: Better query performance and new features
- **Native AOT Support**: Smaller deployment size (optional)
- **Updated C# 13**: Latest language features

---

**Version**: 1.0.0  
**Last Updated**: May 2026  
**.NET Version**: 10.0  
**Angular Version**: 18+

## 🙏 Acknowledgments

- Microsoft for .NET 10 and Entity Framework Core 10
- Angular Team for the excellent framework
- Chart.js for beautiful visualizations
- Material Design for the UI components


