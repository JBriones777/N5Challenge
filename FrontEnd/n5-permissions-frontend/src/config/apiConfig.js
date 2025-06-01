// Challenge/FrontEnd/n5-permissions-frontend/src/config/apiConfig.js
const API_BASE_URL = process.env.REACT_APP_API_URL || 'https://localhost:7277/api'; // Fallback para desarrollo local sin Docker

export default API_BASE_URL;