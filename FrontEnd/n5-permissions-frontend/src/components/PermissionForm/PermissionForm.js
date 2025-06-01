// src/components/PermissionForm/PermissionForm.js
import React, { useState, useEffect } from 'react';
import {
  TextField, Button, Select, MenuItem, FormControl, InputLabel,
  Box, Typography, Grid, CircularProgress, Alert, Paper, IconButton, Tooltip, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import { getPermissionTypes, requestPermission, addNewPermissionType } from '../../services/api'; // Importar addNewPermissionType

const PermissionForm = ({ onSubmitSuccess, onCancel }) => {
  const [nombreEmpleado, setNombreEmpleado] = useState('');
  const [apellidoEmpleado, setApellidoEmpleado] = useState('');
  const [tipoPermisoId, setTipoPermisoId] = useState('');
  
  const [permissionTypes, setPermissionTypes] = useState([]);
  const [loadingTypes, setLoadingTypes] = useState(true);
  const [errorTypes, setErrorTypes] = useState(null);
  
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [errors, setErrors] = useState({}); // Para errores de validación
  const [submitError, setSubmitError] = useState(null); // Para errores del submit general

  const [openAddTypeDialog, setOpenAddTypeDialog] = useState(false);
  const [newPermissionTypeDesc, setNewPermissionTypeDesc] = useState('');
  const [submittingNewType, setSubmittingNewType] = useState(false);
  const [newTypeError, setNewTypeError] = useState('');


  const fetchTypes = async () => {
    setLoadingTypes(true);
    setErrorTypes(null);
    try {
      const types = await getPermissionTypes();
      setPermissionTypes(types);
    } catch (error) {
      console.error("Failed to fetch permission types", error);
      setErrorTypes("Error al cargar tipos de permiso.");
    } finally {
      setLoadingTypes(false);
    }
  };

  useEffect(() => {
    fetchTypes();
  }, []);

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
      const payload = { nombreEmpleado, apellidoEmpleado, tipoPermisoId: Number(tipoPermisoId) };
      await requestPermission(payload);
      onSubmitSuccess();
    } catch (error) {
      console.error("Error submitting permission form", error);
      setSubmitError(error.message || "Error al solicitar el permiso.");
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
      await fetchTypes(); // Recargar tipos
      setTipoPermisoId(addedType.id); // Seleccionar el nuevo tipo
      handleCloseAddTypeDialog();
    } catch (error) {
      console.error("Error adding new permission type", error);
      setNewTypeError(error.message || "Error al agregar el tipo.");
    } finally {
      setSubmittingNewType(false);
    }
  };


  if (loadingTypes && permissionTypes.length === 0) { // Mostrar spinner solo si no hay tipos cargados
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', my: 2 }}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <>
      <Paper elevation={2} sx={{ p: 3, mt: 2 }}>
        <Typography variant="h5" component="h2" gutterBottom sx={{ mb: 2 }}>
          Solicitar Nuevo Permiso
        </Typography>
        {errorTypes && <Alert severity="warning" sx={{ mb: 2 }}>{errorTypes}</Alert>}
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
            <Grid item xs={12} sx={{ display: 'flex', alignItems: 'center' }}>
              <FormControl fullWidth required disabled={isSubmitting || loadingTypes || !!errorTypes} error={!!errors.tipoPermisoId} sx={{ mr: 1 }}>
                <InputLabel id="tipo-permiso-label">Tipo de Permiso</InputLabel>
                <Select
                  name="tipoPermisoId"
                  labelId="tipo-permiso-label"
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
                {errors.tipoPermisoId && <Typography color="error" variant="caption" sx={{ pl: 2, pt:0.5 }}>{errors.tipoPermisoId}</Typography>}
              </FormControl>
              <Tooltip title="Agregar Nuevo Tipo de Permiso">
                <IconButton onClick={handleOpenAddTypeDialog} color="primary" disabled={isSubmitting || loadingTypes}>
                  <AddIcon />
                </IconButton>
              </Tooltip>
            </Grid>
          </Grid>
          {submitError && <Alert severity="error" sx={{ mt: 2 }}>{submitError}</Alert>}
          <Box sx={{ display: 'flex', justifyContent: 'flex-end', gap: 2, mt: 3 }}>
            <Button onClick={onCancel} variant="outlined" color="secondary" disabled={isSubmitting}>
              Cancelar
            </Button>
            <Button type="submit" variant="contained" color="primary" disabled={isSubmitting || loadingTypes}>
              {isSubmitting ? <CircularProgress size={24} color="inherit" /> : 'Solicitar Permiso'}
            </Button>
          </Box>
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
            id="new-type-desc"
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

export default PermissionForm;