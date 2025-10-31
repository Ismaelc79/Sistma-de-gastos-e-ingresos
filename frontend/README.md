# Sistema de Gastos e Ingresos - Frontend

Frontend application for managing income and expenses, built with React, TypeScript, and Tailwind CSS.

## 🚀 Tech Stack

- **React 18** - UI library
- **TypeScript** - Type safety
- **Vite** - Build tool and dev server
- **Tailwind CSS** - Utility-first CSS framework
- **Zustand** - State management
- **React Router v6** - Client-side routing
- **Axios** - HTTP client with JWT interceptors
- **Chart.js** - Data visualization (coming soon)

## 📁 Project Structure

```
frontend/
├── src/
│   ├── app/
│   │   └── routes/          # Routing configuration
│   ├── features/
│   │   ├── auth/            # Authentication (login, register)
│   │   ├── dashboard/       # Dashboard overview
│   │   ├── transactions/    # Transaction management (coming soon)
│   │   ├── reports/         # Reports and charts (coming soon)
│   │   └── profile/         # User profile (coming soon)
│   └── shared/
│       ├── layouts/         # Layout components
│       ├── ui/              # Reusable UI components
│       ├── lib/             # Utilities (axios config)
│       ├── stores/          # Zustand stores
│       └── types/           # TypeScript types
├── public/
└── .env
```

## 🛠️ Getting Started

### Prerequisites

- Node.js 18+ and npm
- Backend API running (see `backend-dotnet` folder)

### Installation

1. Install dependencies:
```bash
npm install
```

2. Create `.env` file (use `.env.example` as template):
```bash
VITE_API_URL=http://localhost:5000/api
```

3. Start development server:
```bash
npm run dev
```

The app will be available at http://localhost:5173

## 📜 Available Scripts

- `npm run dev` - Start development server
- `npm run build` - Build for production
- `npm run preview` - Preview production build

## 🎨 Design System

### Colors

- **Primary (Blue)**: `primary-50` to `primary-950`
- **Dark (Gray/Black)**: `dark-50` to `dark-950`
- Semantic colors: `red`, `green`, `yellow`, `blue`

### Components

- **Button**: Variants (primary, secondary, outline, danger), sizes (sm, md, lg)
- **Input**: With label, error, and helper text
- **Card**: Container with padding options

## 🔐 Authentication

The app uses JWT tokens stored in localStorage:
- `accessToken` - Short-lived token for API requests
- `refreshToken` - Long-lived token for refreshing access

Axios interceptors automatically:
- Add Authorization header to requests
- Refresh expired tokens
- Redirect to login on auth failure

## 🔒 Protected Routes

Routes under `/dashboard`, `/transactions`, `/reports`, and `/profile` require authentication. Unauthenticated users are redirected to `/login`.

## 📝 Features Implemented

- ✅ Login and Register pages with validation
- ✅ JWT authentication with refresh token
- ✅ Protected routes
- ✅ Modern responsive layout with sidebar
- ✅ Dashboard with stats cards
- ⏳ Transactions CRUD (coming soon)
- ⏳ Reports with Chart.js (coming soon)
- ⏳ Profile management (coming soon)

## 🤝 Contributing

Follow the commit conventions defined in `docs/CONVENTIONS.md`.
