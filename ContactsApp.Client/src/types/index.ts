export interface Contact {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  birthDate: string;
  categoryId: number;
  subcategoryId: number | null;
  createdAt: string;
  userId: string;
}

export interface ContactListItem {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
}

export interface ContactCreateDto {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  birthDate: string;
  categoryId: number;
  subcategoryId: number | null;
  customSubcategory?: string;
}

export interface ContactUpdateDto {
  firstName?: string;
  lastName?: string;
  email?: string;
  phoneNumber?: string;
  birthDate?: string;
  categoryId?: number;
  subcategoryId?: number | null;
  customSubcategory?: string;
}

export interface Category {
  id: number;
  name: string;
}

export interface Subcategory {
  id: number;
  name: string;
  categoryId: number;
}

export interface LoginFormData {
  email: string;
  password: string;
}

export interface RegisterFormData {
  username: string;
  email: string;
  password: string;
}

export interface AuthContextType {
  isAuthenticated: boolean;
  login: (token: string) => void;
  logout: () => void;
} 