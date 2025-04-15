// Kategorie kontaktów
export const CATEGORIES = [
  { id: 1, name: 'Służbowy' },
  { id: 2, name: 'Prywatny' },
  { id: 3, name: 'Inny' }
];

// Podkategorie dla kategorii "Służbowy" (id=1)
export const BUSINESS_SUBCATEGORIES = [
  { id: 1, name: 'Szef', categoryId: 1 },
  { id: 2, name: 'Klient', categoryId: 1 },
  { id: 3, name: 'Współpracownik', categoryId: 1 },
  { id: 4, name: 'Partner biznesowy', categoryId: 1 }
];

// Funkcje pomocnicze
export const getCategoryById = (id: number) => {
  return CATEGORIES.find(category => category.id === id);
};

export const getSubcategoriesForCategory = (categoryId: number) => {
  if (categoryId === 1) { // Służbowy
    return BUSINESS_SUBCATEGORIES;
  }
  return []; // Dla innych kategorii nie ma predefiniowanych podkategorii
}; 