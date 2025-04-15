import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { loginUser } from '../api/api';
import { useAuth } from '../context/AuthContext';
import { LoginFormData } from '../types';

const Login: React.FC = () => {
  const [formData, setFormData] = useState<LoginFormData>({
    email: '',
    password: ''
  });
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const { login } = useAuth();
  const navigate = useNavigate();

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    
    // Form validation
    if (!formData.email || !formData.password) {
      setError('Email and password are required');
      return;
    }
    
    try {
      setLoading(true);
      const response = await loginUser(formData);
      
      // Get token from response
      const { token } = response.data;
      
      // Save token and update auth context
      login(token);
      
      // Redirect to home page
      navigate('/');
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || 'Login failed. Please check your credentials.';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={containerStyle}>
      <h1 style={headerStyle}>Login</h1>
      
      {error && <div style={errorStyle}>{error}</div>}
      
      <form onSubmit={handleSubmit} style={formStyle}>
        <div style={fieldStyle}>
          <label htmlFor="email" style={labelStyle}>Email</label>
          <input
            type="email"
            id="email"
            name="email"
            value={formData.email}
            onChange={handleChange}
            style={inputStyle}
            required
          />
        </div>
        
        <div style={fieldStyle}>
          <label htmlFor="password" style={labelStyle}>Password</label>
          <input
            type="password"
            id="password"
            name="password"
            value={formData.password}
            onChange={handleChange}
            style={inputStyle}
            required
          />
        </div>
        
        <button 
          type="submit" 
          style={buttonStyle}
          disabled={loading}
        >
          {loading ? 'Logging in...' : 'Login'}
        </button>
      </form>
      
      <p style={registerTextStyle}>
        Don't have an account? <Link to="/register" style={linkStyle}>Register</Link>
      </p>
    </div>
  );
};

// Basic styles
const containerStyle: React.CSSProperties = {
  padding: '20px',
  maxWidth: '400px',
  margin: '0 auto'
};

const headerStyle: React.CSSProperties = {
  marginBottom: '20px',
  textAlign: 'center'
};

const formStyle: React.CSSProperties = {
  backgroundColor: '#f8f9fa',
  padding: '20px',
  borderRadius: '5px',
  marginBottom: '20px'
};

const fieldStyle: React.CSSProperties = {
  marginBottom: '15px'
};

const labelStyle: React.CSSProperties = {
  display: 'block',
  marginBottom: '5px',
  fontWeight: 'bold'
};

const inputStyle: React.CSSProperties = {
  width: '100%',
  padding: '8px',
  border: '1px solid #ddd',
  borderRadius: '4px',
  fontSize: '16px'
};

const buttonStyle: React.CSSProperties = {
  width: '100%',
  padding: '10px',
  backgroundColor: '#4CAF50',
  color: 'white',
  border: 'none',
  borderRadius: '4px',
  cursor: 'pointer',
  fontSize: '16px'
};

const errorStyle: React.CSSProperties = {
  padding: '10px',
  backgroundColor: '#ffebee',
  color: '#f44336',
  marginBottom: '15px',
  borderRadius: '4px'
};

const registerTextStyle: React.CSSProperties = {
  textAlign: 'center'
};

const linkStyle: React.CSSProperties = {
  color: '#4CAF50',
  textDecoration: 'none'
};

export default Login; 