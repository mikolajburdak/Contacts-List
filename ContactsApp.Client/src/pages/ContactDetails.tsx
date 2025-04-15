import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, Link } from 'react-router-dom';
import { getContactById, deleteContact } from '../api/api';
import { Contact } from '../types';
import { useAuth } from '../context/AuthContext';

const ContactDetails: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [contact, setContact] = useState<Contact | null>(null);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const { isAuthenticated } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    const fetchContact = async () => {
      if (!id) return;
      
      try {
        setLoading(true);
        const response = await getContactById(parseInt(id));
        setContact(response.data);
        setError(null);
      } catch (err) {
        setError('Failed to load contact details. Please try again later.');
        console.error('Error fetching contact:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchContact();
  }, [id]);

  const handleDelete = async () => {
    if (!id || !window.confirm('Are you sure you want to delete this contact?')) {
      return;
    }

    try {
      await deleteContact(parseInt(id));
      navigate('/');
    } catch (err) {
      setError('Failed to delete contact. Please try again later.');
      console.error('Error deleting contact:', err);
    }
  };

  if (loading) {
    return <div style={loadingStyle}>Loading contact details...</div>;
  }

  if (error) {
    return <div style={errorStyle}>{error}</div>;
  }

  if (!contact) {
    return <div style={errorStyle}>Contact not found.</div>;
  }

  // Format date for display
  const formatDate = (dateString: string) => {
    const date = new Date(dateString);
    return date.toLocaleDateString();
  };

  return (
    <div style={containerStyle}>
      <h1 style={headerStyle}>{`${contact.firstName} ${contact.lastName}`}</h1>
      
      <div style={detailsStyle}>
        <div style={fieldStyle}>
          <span style={labelStyle}>Email:</span>
          <span>{contact.email}</span>
        </div>
        
        <div style={fieldStyle}>
          <span style={labelStyle}>Phone:</span>
          <span>{contact.phoneNumber}</span>
        </div>
        
        <div style={fieldStyle}>
          <span style={labelStyle}>Birth Date:</span>
          <span>{formatDate(contact.birthDate)}</span>
        </div>
        
        <div style={fieldStyle}>
          <span style={labelStyle}>Created:</span>
          <span>{formatDate(contact.createdAt)}</span>
        </div>
      </div>
      
      {isAuthenticated && (
        <div style={actionsStyle}>
          <Link to={`/contacts/${contact.id}/edit`} style={editButtonStyle}>
            Edit
          </Link>
          <button onClick={handleDelete} style={deleteButtonStyle}>
            Delete
          </button>
        </div>
      )}
      
      <Link to="/" style={backButtonStyle}>
        Back to Contacts
      </Link>
    </div>
  );
};

// Basic styles
const containerStyle: React.CSSProperties = {
  padding: '20px',
  maxWidth: '800px',
  margin: '0 auto'
};

const headerStyle: React.CSSProperties = {
  marginBottom: '20px'
};

const detailsStyle: React.CSSProperties = {
  backgroundColor: '#f8f9fa',
  padding: '20px',
  borderRadius: '5px',
  marginBottom: '20px'
};

const fieldStyle: React.CSSProperties = {
  marginBottom: '10px',
  display: 'flex'
};

const labelStyle: React.CSSProperties = {
  fontWeight: 'bold',
  width: '100px'
};

const actionsStyle: React.CSSProperties = {
  display: 'flex',
  gap: '10px',
  marginBottom: '20px'
};

const editButtonStyle: React.CSSProperties = {
  padding: '8px 16px',
  backgroundColor: '#4CAF50',
  color: 'white',
  border: 'none',
  borderRadius: '4px',
  cursor: 'pointer',
  textDecoration: 'none',
  textAlign: 'center'
};

const deleteButtonStyle: React.CSSProperties = {
  padding: '8px 16px',
  backgroundColor: '#f44336',
  color: 'white',
  border: 'none',
  borderRadius: '4px',
  cursor: 'pointer'
};

const backButtonStyle: React.CSSProperties = {
  display: 'inline-block',
  marginTop: '10px',
  color: '#333',
  textDecoration: 'none'
};

const loadingStyle: React.CSSProperties = {
  padding: '20px',
  textAlign: 'center'
};

const errorStyle: React.CSSProperties = {
  padding: '20px',
  color: 'red',
  textAlign: 'center'
};

export default ContactDetails; 