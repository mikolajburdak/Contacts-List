import React from 'react';
import { Routes, Route } from 'react-router-dom';
import Navbar from './components/Navbar';
import ContactList from './pages/ContactList';
import ContactDetails from './pages/ContactDetails';
import Login from './pages/Login';
import Register from './pages/Register';
import AddContact from './pages/AddContact';
import EditContact from './pages/EditContact';
import { AuthProvider } from './context/AuthContext';
import ProtectedRoute from './components/ProtectedRoute';

const App: React.FC = () => {
  return (
    <AuthProvider>
      <div className="app">
        <Navbar />
        <div className="container">
          <Routes>
            <Route path="/" element={<ContactList />} />
            <Route path="/contacts/:id" element={<ContactDetails />} />
            <Route path="/login" element={<Login />} />
            <Route path="/register" element={<Register />} />
            <Route 
              path="/contacts/add" 
              element={
                <ProtectedRoute>
                  <AddContact />
                </ProtectedRoute>
              } 
            />
            <Route 
              path="/contacts/:id/edit" 
              element={
                <ProtectedRoute>
                  <EditContact />
                </ProtectedRoute>
              } 
            />
          </Routes>
        </div>
      </div>
    </AuthProvider>
  );
};

export default App; 