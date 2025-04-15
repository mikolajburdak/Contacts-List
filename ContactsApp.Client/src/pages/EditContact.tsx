import React, { useState, useEffect } from 'react';
import { useParams, useNavigate } from 'react-router-dom';
import { getContactById, updateContact } from '../api/api';
import { Contact, ContactUpdateDto, Category, Subcategory } from '../types';
import axios from 'axios';

const API_URL = 'http://localhost:5000/api';

const EditContact: React.FC = () => {
  const { id } = useParams<{ id: string }>();
  const [contact, setContact] = useState<Contact | null>(null);
  const [formData, setFormData] = useState<ContactUpdateDto>({});
  const [categories, setCategories] = useState<Category[]>([]);
  const [subcategories, setSubcategories] = useState<Subcategory[]>([]);
  const [filteredSubcategories, setFilteredSubcategories] = useState<Subcategory[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);
  const navigate = useNavigate();

  // Fetch contact, categories, and subcategories
  useEffect(() => {
    const fetchData = async () => {
      if (!id) return;
      
      try {
        setLoading(true);
        
        // Fetch contact, categories, and subcategories in parallel
        const [contactResponse, categoriesResponse, subcategoriesResponse] = await Promise.all([
          getContactById(parseInt(id)),
          axios.get(`${API_URL}/category`),
          axios.get(`${API_URL}/subcategory`)
        ]);
        
        const contactData = contactResponse.data;
        setContact(contactData);
        
        // Initialize form data from contact
        setFormData({
          firstName: contactData.firstName,
          lastName: contactData.lastName,
          email: contactData.email,
          phoneNumber: contactData.phoneNumber,
          birthDate: contactData.birthDate ? contactData.birthDate.split('T')[0] : '',
          categoryId: contactData.categoryId,
          subcategoryId: contactData.subcategoryId
        });
        
        setCategories(categoriesResponse.data);
        setSubcategories(subcategoriesResponse.data);
        
        // Filter subcategories for the current category
        const filteredSubs = subcategoriesResponse.data.filter(
          (sub: Subcategory) => sub.categoryId === contactData.categoryId
        );
        setFilteredSubcategories(filteredSubs);
      } catch (err) {
        console.error('Error fetching data:', err);
        setError('Failed to load contact data. Please try again later.');
      } finally {
        setLoading(false);
      }
    };
    
    fetchData();
  }, [id]);

  // Filter subcategories when category changes
  useEffect(() => {
    if (formData.categoryId && subcategories.length > 0) {
      const filtered = subcategories.filter(sub => sub.categoryId === formData.categoryId);
      setFilteredSubcategories(filtered);
      
      // Reset subcategory if not in the new filtered list
      if (formData.subcategoryId) {
        const subcategoryExists = filtered.some(sub => sub.id === formData.subcategoryId);
        if (!subcategoryExists) {
          setFormData(prev => ({
            ...prev,
            subcategoryId: null,
            customSubcategory: ''
          }));
        }
      }
    }
  }, [formData.categoryId, subcategories]);

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
    
    if (!id) return;
    
    setError(null);
    
    // Form validation
    if (!formData.firstName || !formData.lastName || !formData.email || !formData.phoneNumber) {
      setError('Please fill in all required fields');
      return;
    }
    
    // Email validation
    if (formData.email) {
      const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
      if (!emailRegex.test(formData.email)) {
        setError('Please enter a valid email address');
        return;
      }
    }
    
    try {
      setLoading(true);
      
      // Create data to send
      const dataToSend = {
        ...formData
      };
      
      // For "Other" category with custom subcategory
      const otherCategoryId = categories.find(c => c.name === 'Other')?.id;
      if (formData.categoryId === otherCategoryId && formData.customSubcategory) {
        // Handle custom subcategory logic
        // This would normally require backend support to create/update a custom subcategory
      }
      
      await updateContact(parseInt(id), dataToSend);
      navigate(`/contacts/${id}`);
    } catch (err: any) {
      const errorMessage = err.response?.data?.message || 'Failed to update contact. Please try again.';
      setError(errorMessage);
    } finally {
      setLoading(false);
    }
  };

  if (loading) {
    return <div style={loadingStyle}>Loading contact data...</div>;
  }

  if (error && error.includes('Failed to load contact data')) {
    return <div style={errorStyle}>{error}</div>;
  }

  if (!contact) {
    return <div style={errorStyle}>Contact not found.</div>;
  }

  const getCategoryName = (id: number | undefined) => {
    if (!id) return '';
    const category = categories.find(c => c.id === id);
    return category ? category.name : '';
  };

  const isOtherCategory = getCategoryName(formData.categoryId) === 'Other';
  const isBusinessCategory = getCategoryName(formData.categoryId) === 'Business';

  return (
    <div style={containerStyle}>
      <h1 style={headerStyle}>Edit Contact</h1>
      
      {error && <div style={errorStyle}>{error}</div>}
      
      <form onSubmit={handleSubmit} style={formStyle}>
        <div style={fieldStyle}>
          <label htmlFor="firstName" style={labelStyle}>First Name*</label>
          <input
            type="text"
            id="firstName"
            name="firstName"
            value={formData.firstName || ''}
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
            value={formData.lastName || ''}
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
            value={formData.email || ''}
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
            value={formData.phoneNumber || ''}
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
            value={formData.birthDate || ''}
            onChange={handleChange}
            style={inputStyle}
          />
        </div>
        
        <div style={fieldStyle}>
          <label htmlFor="categoryId" style={labelStyle}>Category*</label>
          <select
            id="categoryId"
            name="categoryId"
            value={formData.categoryId || ''}
            onChange={handleChange}
            style={inputStyle}
            required
          >
            <option value="">Select Category</option>
            {categories.map(category => (
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
              value={formData.customSubcategory || ''}
              onChange={handleChange}
              style={inputStyle}
              required
            />
          </div>
        )}
        
        <div style={buttonContainerStyle}>
          <button 
            type="submit" 
            style={saveButtonStyle}
            disabled={loading}
          >
            {loading ? 'Saving...' : 'Save Changes'}
          </button>
          
          <button 
            type="button" 
            style={cancelButtonStyle}
            onClick={() => navigate(`/contacts/${id}`)}
          >
            Cancel
          </button>
        </div>
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

const buttonContainerStyle: React.CSSProperties = {
  display: 'flex',
  gap: '10px',
  marginTop: '10px'
};

const saveButtonStyle: React.CSSProperties = {
  flex: '1',
  padding: '10px',
  backgroundColor: '#4CAF50',
  color: 'white',
  border: 'none',
  borderRadius: '4px',
  cursor: 'pointer',
  fontSize: '16px'
};

const cancelButtonStyle: React.CSSProperties = {
  flex: '1',
  padding: '10px',
  backgroundColor: '#f44336',
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

const loadingStyle: React.CSSProperties = {
  padding: '20px',
  textAlign: 'center'
};

export default EditContact; 