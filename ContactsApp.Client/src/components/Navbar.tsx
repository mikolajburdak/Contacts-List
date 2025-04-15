import React from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { useAuth } from '../context/AuthContext';

const Navbar: React.FC = () => {
  const { isAuthenticated, logout } = useAuth();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout();
    navigate('/');
  };

  return (
    <nav style={navbarStyle}>
      <div style={logoStyle}>
        <Link to="/" style={linkStyle}>Contact Manager</Link>
      </div>
      <div style={menuStyle}>
        <Link to="/" style={linkStyle}>Contacts</Link>
        {isAuthenticated ? (
          <>
            <Link to="/contacts/add" style={linkStyle}>Add Contact</Link>
            <button onClick={handleLogout} style={buttonStyle}>Logout</button>
          </>
        ) : (
          <>
            <Link to="/login" style={linkStyle}>Login</Link>
            <Link to="/register" style={linkStyle}>Register</Link>
          </>
        )}
      </div>
    </nav>
  );
};

// Basic styles
const navbarStyle: React.CSSProperties = {
  display: 'flex',
  justifyContent: 'space-between',
  alignItems: 'center',
  padding: '10px 20px',
  backgroundColor: '#f8f9fa',
  borderBottom: '1px solid #ddd'
};

const logoStyle: React.CSSProperties = {
  fontSize: '1.5rem',
  fontWeight: 'bold'
};

const menuStyle: React.CSSProperties = {
  display: 'flex',
  gap: '20px'
};

const linkStyle: React.CSSProperties = {
  textDecoration: 'none',
  color: '#333'
};

const buttonStyle: React.CSSProperties = {
  background: 'none',
  border: 'none',
  cursor: 'pointer',
  fontSize: '1rem',
  color: '#333'
};

export default Navbar; 