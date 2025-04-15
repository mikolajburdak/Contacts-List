import React, { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';
import { getAllContacts } from '../api/api';
import { ContactListItem } from '../types';

const ContactList: React.FC = () => {
  const [contacts, setContacts] = useState<ContactListItem[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchContacts = async () => {
      try {
        setLoading(true);
        const response = await getAllContacts();
        setContacts(response.data);
        setError(null);
      } catch (err) {
        setError('Failed to load contacts. Please try again later.');
        console.error('Error fetching contacts:', err);
      } finally {
        setLoading(false);
      }
    };

    fetchContacts();
  }, []);

  if (loading) {
    return <div style={loadingStyle}>Loading contacts...</div>;
  }

  if (error) {
    return <div style={errorStyle}>{error}</div>;
  }

  return (
    <div style={containerStyle}>
      <h1 style={headerStyle}>Contacts</h1>
      
      {contacts.length === 0 ? (
        <p>No contacts found.</p>
      ) : (
        <div style={listStyle}>
          {contacts.map((contact) => (
            <Link
              key={contact.id}
              to={`/contacts/${contact.id}`}
              style={cardStyle}
            >
              <h3>{`${contact.firstName} ${contact.lastName}`}</h3>
              <p>{contact.email}</p>
            </Link>
          ))}
        </div>
      )}
    </div>
  );
};

// Basic styles
const containerStyle: React.CSSProperties = {
  padding: '20px',
  maxWidth: '1000px',
  margin: '0 auto'
};

const headerStyle: React.CSSProperties = {
  marginBottom: '20px'
};

const listStyle: React.CSSProperties = {
  display: 'grid',
  gridTemplateColumns: 'repeat(auto-fill, minmax(300px, 1fr))',
  gap: '20px'
};

const cardStyle: React.CSSProperties = {
  padding: '15px',
  backgroundColor: '#f8f9fa',
  borderRadius: '5px',
  boxShadow: '0 2px 4px rgba(0,0,0,0.1)',
  textDecoration: 'none',
  color: '#333',
  transition: 'transform 0.2s',
  cursor: 'pointer'
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

export default ContactList; 