// src/components/ModifyForm/ModifyForm.js
import React, { useState, useEffect } from 'react'; // Hooks de React
import {
  TextField, Button, Select, MenuItem, FormControl, InputLabel,
  Box, Typography, Grid, CircularProgress, Alert, Paper, IconButton, Tooltip,
  Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle,
  AppBar, Toolbar
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import CloseIcon from '@mui/icons-material/Close';
// Funciones del servicio API simulado
import {
  getPermissionTypes,
  modifyPermission,
  getPermissionById,
  addNewPermissionType
} from '../../services/api';

const ModifyForm = ({ permissionId, onSubmitSuccess, onCancel, isMobile }) => {
  const [nombreEmpleado, setNombreEmpleado] = useState('');
  const [apellidoEmpleado, setApellidoEmpleado] = useState('');
  const [tipoPermisoId, setTipoPermisoId] = useState('');
  // La fecha fue eliminada según solicitud previa

  const [permissionTypes, setPermissionTypes] = useState([]);
  const [initialLoading, setInitialLoading] = useState(true);
  const [loadingError, setLoadingError] = useState(null);

  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errors, setErrors] = useState({});
  const [submitError, setSubmitError] = useState(null); // Para errores del submit general

  const [openAddTypeDialog, setOpenAddTypeDialog] = useState(false);
  const [newPermissionTypeDesc, setNewPermissionTypeDesc] = useState('');
  const [submittingNewType, setSubmittingNewType] = useState(false);
  const [newTypeError, setNewTypeError] = useState('');

  const fetchTypes = async () => {
    try {
      const typesData = await getPermissionTypes();
      setPermissionTypes(typesData);
    } catch (error) {
      console.error("Error loading permission types for modification", error);
      // No actualizamos loadingError aquí si ya hay un error de carga inicial del permiso
      if (!loadingError) {
        setLoadingError("Error al cargar tipos de permiso.");
      }
    }
  };

  useEffect(() => {
    const loadInitialData = async () => {
      setInitialLoading(true);
      setLoadingError(null);
      setErrors({}); // Resetear errores de validación
      try {
        // Cargar tipos de permiso y datos del permiso en paralelo
        const [typesData, permissionDataResponse] = await Promise.all([
          getPermissionTypes(),
          getPermissionById(permissionId)
        ]);
        
        setPermissionTypes(typesData);
        
        const permissionData = permissionDataResponse; // Asumimos que getPermissionById devuelve el objeto directamente
        setNombreEmpleado(permissionData.nombreEmpleado);
        setApellidoEmpleado(permissionData.apellidoEmpleado);
        setTipoPermisoId(permissionData.tipoPermisoId);
        // La fecha ya no se maneja aquí

      } catch (error) {
        console.error("Error loading data for modification", error);
        setLoadingError(error.message || "Error al cargar datos para modificar.");
      } finally {
        setInitialLoading(false);
      }
    };

    if (permissionId) {
      loadInitialData();
    }
  }, [permissionId]); // Dependencia correcta

  const validateField = (name, value) => {
    let error = '';
    if (name === 'nombreEmpleado' && !value.trim()) {
      error = "El nombre del empleado es requerido.";
    } else if (name === 'nombreEmpleado' && value.length > 100) {
      error = "El nombre no puede exceder los 100 caracteres.";
    }
    if (name === 'apellidoEmpleado' && !value.trim()) {
      error = "El apellido del empleado es requerido.";
    } else if (name === 'apellidoEmpleado' && value.length > 100) {
      error = "El apellido no puede exceder los 100 caracteres.";
    }
    if (name === 'tipoPermisoId' && !value) {
      error = "Debe seleccionar un tipo de permiso.";
    }
    return error;
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    if (name === 'nombreEmpleado') setNombreEmpleado(value);
    if (name === 'apellidoEmpleado') setApellidoEmpleado(value);
    if (name === 'tipoPermisoId') setTipoPermisoId(value);

    // Validar al cambiar si ya había un error para ese campo
    if (errors[name]) {
      setErrors(prev => ({ ...prev, [name]: validateField(name, value) }));
    }
  };

  const handleBlur = (e) => {
    const { name, value } = e.target;
    setErrors(prev => ({ ...prev, [name]: validateField(name, value) }));
  };

  const validateForm = () => {
    const newErrors = {
      nombreEmpleado: validateField('nombreEmpleado', nombreEmpleado),
      apellidoEmpleado: validateField('apellidoEmpleado', apellidoEmpleado),
      tipoPermisoId: validateField('tipoPermisoId', tipoPermisoId),
    };
    setErrors(newErrors);
    return Object.values(newErrors).every(error => !error); // Retorna true si no hay errores
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    setSubmitError(null); // Limpiar error de submit general
    if (!validateForm()) {
      return;
    }

    setIsSubmitting(true);
    try {
      const payload = { nombreEmpleado, apellidoEmpleado, tipoPermisoId: Number(tipoPermisoId) }; // Sin fecha
      await modifyPermission(permissionId, payload);
      onSubmitSuccess(); // Esta función es pasada por App.js para cerrar el diálogo y recargar
    } catch (error) {
      console.error("Error submitting modification form", error);
      setSubmitError(error.message || "Error al modificar el permiso.");
    } finally {
      setIsSubmitting(false);
    }
  };

  const handleOpenAddTypeDialog = () => setOpenAddTypeDialog(true);
  const handleCloseAddTypeDialog = () => {
    setOpenAddTypeDialog(false);
    setNewPermissionTypeDesc('');
    setNewTypeError('');
  };

  const handleAddNewPermissionType = async () => {
    if (!newPermissionTypeDesc.trim()) {
      setNewTypeError("La descripción no puede estar vacía.");
      return;
    }
    setSubmittingNewType(true);
    setNewTypeError('');
    try {
      const addedType = await addNewPermissionType(newPermissionTypeDesc);
      await fetchTypes(); // Recargar tipos de permiso en el select
      setTipoPermisoId(addedType.id); // Seleccionar el nuevo tipo automáticamente
      handleCloseAddTypeDialog();
    } catch (error) {
      console.error("Error adding new permission type", error);
      setNewTypeError(error.message || "Error al agregar el tipo.");
    } finally {
      setSubmittingNewType(false);
    }
  };

  if (initialLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', p: isMobile ? 2 : 4, minHeight: 200 }}> {/* Darle algo de altura */}
        <CircularProgress />
      </Box>
    );
  }
  if (loadingError) {
    return (
        <Box sx={{p: isMobile ? 2 : 3}}>
            <Alert severity="error">{loadingError}</Alert>
            <Button onClick={onCancel} sx={{mt: 2}}>Cerrar</Button>
        </Box>
    );
  }

  return (
    <>
      {isMobile && (
        <AppBar sx={{ position: 'relative' }}>
          <Toolbar>
            <IconButton edge="start" color="inherit" onClick={onCancel} aria-label="close">
              <CloseIcon />
            </IconButton>
            <Typography sx={{ ml: 2, flex: 1 }} variant="h6" component="div">
              Modificar Permiso
            </Typography>
            <Button autoFocus color="inherit" onClick={handleSubmit} disabled={isSubmitting}>
              {isSubmitting ? <CircularProgress size={24} color="inherit" /> : 'Guardar'}
            </Button>
          </Toolbar>
        </AppBar>
      )}
      {/* En modo no móvil, el Paper da el padding y el fondo. En móvil, el AppBar maneja el título/acciones */}
      <Paper elevation={isMobile ? 0 : 2} sx={{ p: isMobile ? 2 : 3, mt: isMobile ? 0 : 0 }}>
        {!isMobile && (
            <Typography variant="h5" component="h2" gutterBottom sx={{ mb: 2 }}>
                Modificar Permiso (ID: {permissionId})
            </Typography>
        )}
        <Box component="form" onSubmit={handleSubmit} noValidate>
          <Grid container spacing={2}>
            <Grid item xs={12} sm={6}>
              <TextField
                name="nombreEmpleado"
                label="Nombre Empleado"
                value={nombreEmpleado}
                onChange={handleChange}
                onBlur={handleBlur}
                fullWidth
                required
                disabled={isSubmitting}
                error={!!errors.nombreEmpleado}
                helperText={errors.nombreEmpleado}
              />
            </Grid>
            <Grid item xs={12} sm={6}>
              <TextField
                name="apellidoEmpleado"
                label="Apellido Empleado"
                value={apellidoEmpleado}
                onChange={handleChange}
                onBlur={handleBlur}
                fullWidth
                required
                disabled={isSubmitting}
                error={!!errors.apellidoEmpleado}
                helperText={errors.apellidoEmpleado}
              />
            </Grid>
            <Grid item xs={12} sx={{ display: 'flex', alignItems: 'flex-start' }}> {/* alignItems a flex-start para el helperText del Select */}
              <FormControl fullWidth required disabled={isSubmitting || permissionTypes.length === 0} error={!!errors.tipoPermisoId} sx={{ mr: 1 }}>
                <InputLabel id="tipo-permiso-label-modify">Tipo de Permiso</InputLabel>
                <Select
                  name="tipoPermisoId"
                  labelId="tipo-permiso-label-modify"
                  value={tipoPermisoId}
                  label="Tipo de Permiso"
                  onChange={handleChange}
                  onBlur={handleBlur} // Podrías querer validación en blur para select también
                >
                  <MenuItem value="" disabled><em>Seleccione un tipo</em></MenuItem>
                  {permissionTypes.map((type) => (
                    <MenuItem key={type.id} value={type.id}>
                      {type.descripcion}
                    </MenuItem>
                  ))}
                </Select>
                {/* Mostrar helperText debajo del Select */}
                {errors.tipoPermisoId && <Typography color="error" variant="caption" sx={{ ml: "14px", mt: "3px" }}>{errors.tipoPermisoId}</Typography>}
              </FormControl>
              <Tooltip title="Agregar Nuevo Tipo de Permiso">
                {/* Span para que el Tooltip funcione con botón deshabilitado */}
                <span> 
                  <IconButton onClick={handleOpenAddTypeDialog} color="primary" disabled={isSubmitting}>
                    <AddIcon />
                  </IconButton>
                </span>
              </Tooltip>
            </Grid>
            {/* Campo de Fecha Eliminado */}
          </Grid>
          {submitError && <Alert severity="error" sx={{ mt: 2 }}>{submitError}</Alert>}
          {!isMobile && (
            <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 2, mt: 3 }}>
              <Button onClick={onCancel} variant="outlined" color="secondary" disabled={isSubmitting}>
                Cancelar
              </Button>
              <Button type="submit" variant="contained" color="primary" disabled={isSubmitting}>
                {isSubmitting ? <CircularProgress size={24} color="inherit" /> : 'Guardar Cambios'}
              </Button>
            </Box>
          )}
        </Box>
      </Paper>

      <Dialog open={openAddTypeDialog} onClose={handleCloseAddTypeDialog}>
        <DialogTitle>Agregar Nuevo Tipo de Permiso</DialogTitle>
        <DialogContent>
          <DialogContentText sx={{mb:2}}>
            Ingrese la descripción para el nuevo tipo de permiso.
          </DialogContentText>
          <TextField
            autoFocus
            margin="dense"
            id="new-type-desc-modify"
            label="Descripción del Tipo"
            type="text"
            fullWidth
            variant="standard"
            value={newPermissionTypeDesc}
            onChange={(e) => setNewPermissionTypeDesc(e.target.value)}
            error={!!newTypeError}
            helperText={newTypeError}
            disabled={submittingNewType}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseAddTypeDialog} disabled={submittingNewType} color="secondary">Cancelar</Button>
          <Button onClick={handleAddNewPermissionType} disabled={submittingNewType} variant="contained">
            {submittingNewType ? <CircularProgress size={20} /> : "Agregar"}
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
};

export default ModifyForm;