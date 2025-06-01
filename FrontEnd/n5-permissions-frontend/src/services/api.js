
import axios from 'axios';
import API_BASE_URL from '../config/apiConfig'; 


const apiClient = axios.create({
  baseURL: API_BASE_URL,
  headers: {
    'Content-Type': 'application/json',
  },
});


const handleError = (error, operation = "operation") => {
  console.error(`API Error during ${operation}:`, error);
  let errorMessage = `Error durante ${operation}: `;

  if (error.response) {
   
    console.error("Data:", error.response.data);
    console.error("Status:", error.response.status);
    
    errorMessage += `Error del servidor ${error.response.status}. `;
    if (error.response.data && error.response.data.message) {
      errorMessage += error.response.data.message;
      if (error.response.data.details && error.response.data.details.length > 0 && error.response.data.details[0].message) {
        errorMessage += ` (${error.response.data.details[0].message})`;
      }
    } else if (typeof error.response.data === 'string' && error.response.data.length > 0 && error.response.data.length < 200) {
        errorMessage += error.response.data;
    } else {
      errorMessage += `(Respuesta no contiene mensaje detallado).`;
    }
  } else if (error.request) {
    
    console.error("Request (no response received):", error.request);
    errorMessage = `No se recibió respuesta del servidor para ${operation}. Verifica la conexión, la URL de la API (${API_BASE_URL}) y si el backend está ejecutándose y accesible.`;
  } else {
    
    console.error('Error setting up request:', error.message);
    errorMessage = `Error al configurar la solicitud para ${operation}: ${error.message}`;
  }
  throw new Error(errorMessage);
};

export const getPermissions = async () => {
  try {
    console.log(`API: GET ${API_BASE_URL}/permission`);
    const response = await apiClient.get('/permission');
    if (!Array.isArray(response.data)) {
        console.error("getPermissions: La respuesta de la API no es un array:", response.data);
        throw new Error("Formato de respuesta inesperado al obtener permisos.");
    }
    return response.data.map(p => ({
        ...p,
        tipoPermisoDescripcion: p.tipoPermiso,
    }));
  } catch (error) {
    
    throw error; 
  }
};

export const getPermissionTypes = async () => {
  try {
    console.log(`API: GET ${API_BASE_URL}/permissionType`);
    const response = await apiClient.get('/permissionType');
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const addNewPermissionType = async (descriptionParam) => {
  try {
    const payload = { descripcion: descriptionParam };
    console.log(`API: POST ${API_BASE_URL}/permissionType`, payload);
    const response = await apiClient.post('/permissionType', payload);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const requestPermission = async (permissionData) => {
  const payload = {
    nombreEmpleado: permissionData.nombreEmpleado,
    apellidoEmpleado: permissionData.apellidoEmpleado,
    tipoPermisoId: permissionData.tipoPermisoId,
  };
  try {
    console.log(`API: POST ${API_BASE_URL}/permission`, payload);
    const response = await apiClient.post('/permission', payload);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const modifyPermission = async (id, permissionData) => {
  const payload = {
    id: id,
    nombreEmpleado: permissionData.nombreEmpleado,
    apellidoEmpleado: permissionData.apellidoEmpleado,
    tipoPermisoId: permissionData.tipoPermisoId,
  };
  try {
    console.log(`API: PUT ${API_BASE_URL}/permission`, payload);
    const response = await apiClient.put('/permission', payload);
    return response.data;
  } catch (error) {
    throw error;
  }
};

export const getPermissionById = async (id) => {
  try {
    console.log(`API: GET ${API_BASE_URL}/permission/${id}`);
    const response = await apiClient.get(`/permission/${id}`);
    const permissionData = Array.isArray(response.data) ? response.data[0] : response.data;
    if (!permissionData) {
        
        throw new Error(`Permiso con ID ${id} no encontrado en la respuesta.`);
    }
    return permissionData;
  } catch (error) {
    throw error;
  }
};