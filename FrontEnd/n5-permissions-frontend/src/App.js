
import React, { useState, useEffect, useCallback } from 'react';
import {
  Container, Typography, Box, CircularProgress, Alert, Button,
  Paper, AppBar, Toolbar, Snackbar, Dialog, useTheme, useMediaQuery, Fab,
  Fade
} from '@mui/material';
import AddCircleOutlineIcon from '@mui/icons-material/AddCircleOutline';
import AddIcon from '@mui/icons-material/Add';
import PermissionsTable from './components/PermissionsTable/PermissionsTable';
import PermissionForm from './components/PermissionForm/PermissionForm';
import ModifyForm from './components/ModifyForm/ModifyForm';
import { getPermissions as fetchPermissionsApi } from './services/api';

const App = () => {
 
  const [permissions, setPermissions] = useState([]);
  const [currentView, setCurrentView] = useState('table');
  const [editingPermissionId, setEditingPermissionId] = useState(null);
  const [openEditDialog, setOpenEditDialog] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  const [error, setError] = useState(null);
  const [snackbar, setSnackbar] = useState({ open: false, message: '', severity: 'success' });

  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  const loadPermissions = useCallback(async () => {
    setIsLoading(true);
    setError(null);
    try {
      const data = await fetchPermissionsApi();
      setPermissions(data);
    } catch (err) {
      console.error("Error al cargar permisos:", err);
      const errorMessage = err.message || 'Error al cargar los permisos.';
      setError(errorMessage);
      
    } finally {
      setIsLoading(false);
    }
  }, []); 

  useEffect(() => {
    loadPermissions();
  }, [loadPermissions]);

  const showSnackbar = (message, severity = 'success') => {
    setSnackbar({ open: true, message, severity });
  };

  const handleCreateFormSubmitSuccess = () => {
    setCurrentView('table');
    loadPermissions();
    showSnackbar(`Permiso solicitado exitosamente.`, 'success');
  };

  const handleEditFormSubmitSuccess = () => {
    setOpenEditDialog(false);
    setEditingPermissionId(null);
    loadPermissions();
    showSnackbar(`Permiso modificado exitosamente.`, 'success');
  };

  const handleEditClick = (permission) => {
    setEditingPermissionId(permission.id);
    setOpenEditDialog(true);
  };

  const handleAddNewClick = () => {
    setCurrentView('create');
    setEditingPermissionId(null);
  };

  const handleCancelCreateForm = () => {
    setCurrentView('table');
  };

  const handleCancelEditDialog = () => {
    setOpenEditDialog(false);
    setEditingPermissionId(null);
  };

  const renderMainContent = () => {
    if (currentView === 'create') {
      return (
        <PermissionForm
          onSubmitSuccess={handleCreateFormSubmitSuccess}
          onCancel={handleCancelCreateForm}
        />
      );
    }

    if (isLoading && permissions.length === 0) {
      return <PermissionsTable permissions={null} isLoading={true} />;
    }

    if (error && permissions.length === 0) {
      return <Alert severity="error" sx={{ m: 2 }}>{error}</Alert>;
    }
    
    const dataAvailable = permissions.length > 0;

    return (
      <>
        {!isMobile && currentView === 'table' && (
          <Box sx={{ display: 'flex', justifyContent: 'flex-end', mb: 2 }}>
            <Button
              variant="contained"
              color="primary"
              startIcon={<AddCircleOutlineIcon />}
              onClick={handleAddNewClick}
              disabled={isLoading && dataAvailable} 
            >
              Solicitar Nuevo Permiso
            </Button>
          </Box>
        )}

        {}
        <Fade in={!isLoading || dataAvailable} timeout={isLoading && !dataAvailable ? 0 : 500}>
            <div>
                <PermissionsTable
                    permissions={permissions} 
                    onEdit={handleEditClick}
                    isLoading={isLoading && !dataAvailable} 
                />
            </div>
        </Fade>
      </>
    );
  };

  return (
    <>
      <AppBar position="static" color="primary"> {}
        <Toolbar>
          <Typography 
            variant="h6" 
            component="div" 
            sx={{ flexGrow: 1, color: 'common.white' }} 
          >
            Gestión de Permisos
          </Typography>
        </Toolbar>
      </AppBar>
      <Container maxWidth="lg" sx={{ mt: {xs: 2, md: 4}, mb: {xs: 2, md: 4} }}>
        <Paper elevation={0} sx={{ p: { xs: 1, sm: 2, md: 3 }, backgroundColor: 'transparent' }}>
          {renderMainContent()}
        </Paper>
      </Container>

      {isMobile && currentView === 'table' && (
        <Fab
            color="primary"
            aria-label="add"
            onClick={handleAddNewClick}
            sx={{
                position: 'fixed',
                bottom: 16,
                right: 16,
            }}
        >
            <AddIcon />
        </Fab>
      )}

      <Dialog 
        open={openEditDialog} 
        onClose={handleCancelEditDialog} 
        maxWidth="md"
        fullWidth
        fullScreen={isMobile}
      >
        {editingPermissionId && (
          <ModifyForm
            permissionId={editingPermissionId}
            onSubmitSuccess={handleEditFormSubmitSuccess}
            onCancel={handleCancelEditDialog}
            isMobile={isMobile}
          />
        )}
      </Dialog>

      <Snackbar
        open={snackbar.open}
        autoHideDuration={6000}
        onClose={() => setSnackbar({ ...snackbar, open: false })}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
      >
        <Alert onClose={() => setSnackbar({ ...snackbar, open: false })} severity={snackbar.severity || 'info'} sx={{ width: '100%' }}>
          {snackbar.message}
        </Alert>
      </Snackbar>
    </>
  );
};

export default App;