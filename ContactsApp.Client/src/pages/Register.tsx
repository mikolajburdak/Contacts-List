import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import { registerUser } from '../api/api';
import { RegisterFormData } from '../types';

const Register: React.FC = () => {
  const [formData, setFormData] = useState<RegisterFormData>({
    username: '',
    email: '',
    password: ''
  });
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
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
    if (!formData.username || !formData.email || !formData.password) {
      setError('All fields are required');
      return;
    }
    
    // Password validation
    if (formData.password.length < 6) {
      setError('Password must be at least 6 characters long');
      return;
    }
    
    try {
      setLoading(true);
      await registerUser(formData);
      
      // Redirect to login page after successful registration
      navigate('/login', { state: { message: 'Registration successful. Please log in.' } });
    } catch (err: any) {
      const errorMessage = 
        err.response?.data?.message || 
        err.response?.data?.errors?.join(', ') || 
        'Registration failed. Please try again.';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div style={containerStyle}>
      <h1 style={headerStyle}>Register</h1>
      
      {error && <div style={errorStyle}>{error}</div>}
      
      <form onSubmit={handleSubmit} style={formStyle}>
        <div style={fieldStyle}>
          <label htmlFor="username" style={labelStyle}>Username</label>
          <input
            type="text"
            id="username"
            name="username"
            value={formData.username}
            onChange={handleChange}
            style={inputStyle}
            required
          />
        </div>
        
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
          <small style={hintStyle}>Password must be at least 6 characters long.</small>
        </div>
        
        <button 
          type="submit" 
          style={buttonStyle}
          disabled={loading}
        >
          {loading ? 'Registering...' : 'Register'}
        </button>
      </form>
      
      <p style={loginTextStyle}>
        Already have an account? <Link to="/login" style={linkStyle}>Login</Link>
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

const hintStyle: React.CSSProperties = {
  display: 'block',
  color: '#666',
  fontSize: '12px',
  marginTop: '5px'
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

const loginTextStyle: React.CSSProperties = {
  textAlign: 'center'
};

const linkStyle: React.CSSProperties = {
  color: '#4CAF50',
  textDecoration: 'none'
};

export default Register; 