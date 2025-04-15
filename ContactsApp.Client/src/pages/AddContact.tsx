import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { createContact } from '../api/api';
import { ContactCreateDto } from '../types';
import { CATEGORIES, BUSINESS_SUBCATEGORIES, getSubcategoriesForCategory } from '../constants/categories';

const AddContact: React.FC = () => {
  const [formData, setFormData] = useState<ContactCreateDto>({
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    birthDate: '',
    categoryId: 0,
    subcategoryId: null,
    customSubcategory: ''
  });
  
  const [filteredSubcategories, setFilteredSubcategories] = useState<any[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState<boolean>(false);
  const navigate = useNavigate();

  // Set default category when component mounts
  useEffect(() => {
    if (CATEGORIES.length > 0) {
      setFormData(prev => ({
        ...prev,
        categoryId: CATEGORIES[0].id
      }));
      
      // Load subcategories for the default category
      setFilteredSubcategories(getSubcategoriesForCategory(CATEGORIES[0].id));
    }
  }, []);

  // Update subcategories when category changes
  useEffect(() => {
    if (formData.categoryId) {
      const subcats = getSubcategoriesForCategory(formData.categoryId);
      setFilteredSubcategories(subcats);
      
      // Reset subcategory when category changes
      setFormData(prev => ({
        ...prev,
        subcategoryId: null,
        customSubcategory: ''
      }));
    }
  }, [formData.categoryId]);

  const handleChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLSelectElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    
    if (name === 'categoryId') {
      setFormData(prev => ({
        ...prev,
        [name]: parseInt(value)
      }));
    } else if (name === 'subcategoryId') {
      setFormData(prev => ({
        ...prev,
        [name]: value === '' ? null : parseInt(value)
      }));
    } else {
      setFormData(prev => ({
        ...prev,
        [name]: value
      }));
    }
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    
    // Form validation
    if (!formData.firstName || !formData.lastName || !formData.email || !formData.phoneNumber) {
      setError('Please fill in all required fields');
      return;
    }
    
    // Email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) {
      setError('Please enter a valid email address');
      return;
    }
    
    try {
      setLoading(true);
      await createContact(formData);
      navigate('/');
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || 'Failed to create contact. Please try again.';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  // Helper function to check if current category is "Służbowy"
  const isBusinessCategory = formData.categoryId === 1; // ID 1 is "Służbowy"
  
  // Helper function to check if current category is "Inny"
  const isOtherCategory = formData.categoryId === 3; // ID 3 is "Inny"

  return (
    <div style={containerStyle}>
      <h1 style={headerStyle}>Add New Contact</h1>
      
      {error && <div style={errorStyle}>{error}</div>}
      
      <form onSubmit={handleSubmit} style={formStyle}>
        <div style={fieldStyle}>
          <label htmlFor="firstName" style={labelStyle}>First Name*</label>
          <input
            type="text"
            id="firstName"
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            style={inputStyle}
            required
          />
        </div>
        
        <div style={fieldStyle}>
          <label htmlFor="lastName" style={labelStyle}>Last Name*</label>
          <input
            type="text"
            id="lastName"
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            style={inputStyle}
            required
          />
        </div>
        
        <div style={fieldStyle}>
          <label htmlFor="email" style={labelStyle}>Email*</label>
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
          <label htmlFor="phoneNumber" style={labelStyle}>Phone Number*</label>
          <input
            type="tel"
            id="phoneNumber"
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            style={inputStyle}
            required
          />
        </div>
        
        <div style={fieldStyle}>
          <label htmlFor="birthDate" style={labelStyle}>Birth Date</label>
          <input
            type="date"
            id="birthDate"
            name="birthDate"
            value={formData.birthDate}
            onChange={handleChange}
            style={inputStyle}
          />
        </div>
        
        <div style={fieldStyle}>
          <label htmlFor="categoryId" style={labelStyle}>Category*</label>
          <select
            id="categoryId"
            name="categoryId"
            value={formData.categoryId}
            onChange={handleChange}
            style={inputStyle}
            required
          >
            <option value="">Select Category</option>
            {CATEGORIES.map(category => (
              <option key={category.id} value={category.id}>
                {category.name}
              </option>
            ))}
          </select>
        </div>
        
        {isBusinessCategory && (
          <div style={fieldStyle}>
            <label htmlFor="subcategoryId" style={labelStyle}>Business Type*</label>
            <select
              id="subcategoryId"
              name="subcategoryId"
              value={formData.subcategoryId || ''}
              onChange={handleChange}
              style={inputStyle}
              required
            >
              <option value="">Select Business Type</option>
              {filteredSubcategories.map(subcategory => (
                <option key={subcategory.id} value={subcategory.id}>
                  {subcategory.name}
                </option>
              ))}
            </select>
          </div>
        )}
        
        {isOtherCategory && (
          <div style={fieldStyle}>
            <label htmlFor="customSubcategory" style={labelStyle}>Custom Category*</label>
            <input
              type="text"
              id="customSubcategory"
              name="customSubcategory"
              value={formData.customSubcategory}
              onChange={handleChange}
              style={inputStyle}
              required
            />
          </div>
        )}
        
        <button 
          type="submit" 
          style={buttonStyle}
          disabled={loading}
        >
          {loading ? 'Creating...' : 'Create Contact'}
        </button>
      </form>
    </div>
  );
};

// Basic styles
const containerStyle: React.CSSProperties = {
  padding: '20px',
  maxWidth: '600px',
  margin: '0 auto'
};

const headerStyle: React.CSSProperties = {
  marginBottom: '20px'
};

const formStyle: React.CSSProperties = {
  backgroundColor: '#f8f9fa',
  padding: '20px',
  borderRadius: '5px'
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
  fontSize: '16px',
  marginTop: '10px'
};

const errorStyle: React.CSSProperties = {
  padding: '10px',
  backgroundColor: '#ffebee',
  color: '#f44336',
  marginBottom: '15px',
  borderRadius: '4px'
};

export default AddContact; 