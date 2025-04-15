# Contacts App Frontend

A simple React + TypeScript frontend for the Contacts App.

## Technical Specification

### Technologies Used
- React 18
- TypeScript
- React Router v6
- Axios for API requests
- Webpack for bundling

### Components Overview

#### Pages
- `ContactList`: Displays all contacts, accessible without login
- `ContactDetails`: Shows detailed information about a single contact
- `Login`: User login interface
- `Register`: New user registration interface
- `AddContact`: Form to add a new contact (requires authentication)
- `EditContact`: Form to edit an existing contact (requires authentication)

#### Components
- `Navbar`: Navigation bar with conditional links based on authentication status
- `ProtectedRoute`: Route wrapper that redirects unauthenticated users to login

#### Context
- `AuthContext`: Manages authentication state across the application

### API Service
API communication is handled through Axios instance with automatic token handling.

## Setup Instructions

1. Make sure the backend API (ContactsApp.Api) is running on port 5000

2. Install dependencies:
   ```
   npm install
   ```

3. Start the development server:
   ```
   npm start
   ```

4. Build for production:
   ```
   npm run build
   ```

## Features
- View list of contacts without login
- User authentication for edit/add/delete operations
- Create, read, update, and delete contacts
- Categorize contacts by type
- Custom categories for "Other" type 