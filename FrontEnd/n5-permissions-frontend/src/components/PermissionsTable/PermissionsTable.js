// src/components/PermissionsTable/PermissionsTable.js
import React from 'react';
import {
  Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
  Paper, IconButton, Typography, Box, Tooltip, useTheme, useMediaQuery,
  Skeleton
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import { format, isValid, parseISO } from 'date-fns'; // Asegúrate de tener isValid y parseISO
import { es } from 'date-fns/locale';

const PermissionsTable = ({ permissions, onEdit, isLoading }) => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('sm'));

  // --- ESQUELETO DE CARGA ---
  if (isLoading && (!permissions || permissions.length === 0)) {
    const skeletonRows = isMobile ? 3 : 5;
    return (
      <TableContainer component={Paper} sx={{ mt: 2, boxShadow: 3 }}>
        <Table aria-label="tabla de permisos cargando" size={isMobile ? "small" : "medium"}>
          <TableHead sx={{ backgroundColor: 'primary.main' }}>
            <TableRow>
              {!isMobile && <TableCell sx={{ color: 'common.white', width: '10%' }}><Skeleton variant="text" sx={{ bgcolor: 'primary.light' }} /></TableCell>}
              <TableCell sx={{ color: 'common.white', width: isMobile ? '40%' : '30%' }}><Skeleton variant="text" sx={{ bgcolor: 'primary.light' }} /></TableCell>
              <TableCell sx={{ color: 'common.white', width: isMobile ? '30%' : '30%' }}><Skeleton variant="text" sx={{ bgcolor: 'primary.light' }} /></TableCell>
              <TableCell sx={{ color: 'common.white', width: isMobile ? '15%' : '15%' }}><Skeleton variant="text" sx={{ bgcolor: 'primary.light' }} /></TableCell>
              <TableCell sx={{ color: 'common.white', width: isMobile ? '15%' : '15%', textAlign: 'center' }}><Skeleton variant="text" sx={{ bgcolor: 'primary.light' }} /></TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {Array.from(new Array(skeletonRows)).map((_, index) => (
              <TableRow key={`skeleton-${index}`} sx={{ '&:nth-of-type(odd)': { backgroundColor: 'action.hover' } }}>
                {!isMobile && <TableCell><Skeleton variant="text" /></TableCell>}
                <TableCell><Skeleton variant="text" /></TableCell>
                <TableCell><Skeleton variant="text" /></TableCell>
                <TableCell><Skeleton variant="text" /></TableCell>
                <TableCell align="center"><Skeleton variant="circular" width={isMobile ? 24 : 32} height={isMobile ? 24 : 32} /></TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    );
  }
  // --- FIN ESQUELETO DE CARGA ---

  if (!permissions || permissions.length === 0) {
    return (
      <Box sx={{ textAlign: 'center', mt: 4, p: 2 }}>
        <Typography variant="subtitle1">No hay permisos registrados actualmente.</Typography>
      </Box>
    );
  }

  const columns = [
    { id: 'id', label: 'ID', minWidth: 50, numeric: true, hideOnMobile: true },
    { id: 'empleado', label: 'Empleado', minWidth: isMobile ? 100 : 170 },
    { id: 'tipo', label: 'Tipo', minWidth: isMobile ? 80 : 150 },
    { id: 'fecha', label: 'Fecha', minWidth: isMobile ? 65 : 100, align: 'left' },
    { id: 'accion', label: 'Acción', minWidth: isMobile ? 40 : 70, align: 'center' },
  ];

  const visibleColumns = columns.filter(column => !(isMobile && column.hideOnMobile));

  return (
    <TableContainer component={Paper} sx={{ mt: 2, boxShadow: 3 }}>
      <Table aria-label="tabla de permisos" size={isMobile ? "small" : "medium"}>
        <TableHead sx={{ backgroundColor: 'primary.main' }}>
          <TableRow>
            {visibleColumns.map((column) => (
              <TableCell
                key={column.id}
                align={column.align || 'left'}
                sx={{
                    minWidth: column.minWidth, 
                    color: 'common.white',
                    fontWeight: 'bold',
                    padding: isMobile ? '8px 6px' : '16px'
                }}
              >
                {column.label}
              </TableCell>
            ))}
          </TableRow>
        </TableHead>
        <TableBody>
          {permissions.map((permission) => (
            <TableRow
              key={permission.id}
              hover
              sx={{ '&:nth-of-type(odd)': { backgroundColor: 'action.hover' } }}
            >
              {visibleColumns.map((column) => {
                let content;
                switch (column.id) {
                  case 'id':
                    content = <Typography variant="body2">{permission.id}</Typography>;
                    break;
                  case 'empleado':
                    content = <Typography variant="body2">{`${permission.nombreEmpleado || ''} ${permission.apellidoEmpleado || ''}`.trim()}</Typography>;
                    break;
                  case 'tipo':
                    content = <Typography variant="body2">{permission.tipoPermisoDescripcion || 'N/A'}</Typography>;
                    break;
                  case 'fecha':
                    let dateToDisplay = 'N/A'; // Valor por defecto
                    if (permission.fechaPermiso) {
                      // Intenta parsear la fecha. Funciona bien con formatos ISO 8601.
                      const parsedDate = parseISO(permission.fechaPermiso);
                      if (isValid(parsedDate)) {
                        dateToDisplay = format(parsedDate, isMobile ? 'dd/MM/yy' : 'dd/MM/yyyy', { locale: es });
                      } else {
                        // Fallback si parseISO falla, intenta con new Date (menos confiable para strings variados)
                        const fallbackDate = new Date(permission.fechaPermiso);
                        if (isValid(fallbackDate)) {
                           dateToDisplay = format(fallbackDate, isMobile ? 'dd/MM/yy' : 'dd/MM/yyyy', { locale: es });
                        } else {
                           console.warn(`Invalid date value received for permission ID ${permission.id}:`, permission.fechaPermiso);
                        }
                      }
                    }
                    content = (
                      <Typography 
                        variant={isMobile ? "caption" : "body2"}
                        color={isMobile ? "text.secondary" : "inherit"}
                      >
                        {dateToDisplay}
                      </Typography>
                    );
                    break;
                  case 'accion':
                    return (
                      <TableCell key={column.id} align={column.align} sx={{ padding: isMobile ? '4px 0px' : '16px' }}>
                        <Tooltip title="Modificar Permiso">
                          <IconButton
                            color="secondary"
                            onClick={() => onEdit(permission)}
                            aria-label={`modificar permiso ${permission.id}`}
                            size={isMobile ? "small" : "medium"}
                          >
                            <EditIcon fontSize={isMobile ? "small" : "inherit"} />
                          </IconButton>
                        </Tooltip>
                      </TableCell>
                    );
                  default:
                    content = '';
                }
                return (
                  <TableCell 
                    key={column.id} 
                    align={column.align || 'left'} 
                    sx={{ padding: isMobile ? '8px 6px' : '16px' }}
                  >
                    {content}
                  </TableCell>
                );
              })}
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </TableContainer>
  );
};

export default PermissionsTable;