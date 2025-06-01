
import { createTheme } from '@mui/material/styles';
import { red } from '@mui/material/colors';

const theme = createTheme({
  palette: {
    primary: {
      main: '#1976d2', 
    },
    secondary: {
      main: '#dc004e', 
    },
    error: {
      main: red.A400,
    },
    background: {
      default: '#f4f6f8', 
    },
  },
  typography: {
    fontFamily: 'Roboto, Arial, sans-serif',
    h5: {
        fontWeight: 500,
    }
  },
  shape: {
    borderRadius: 8, 
  },
  components: {
    MuiAppBar: {
      styleOverrides: {
        colorPrimary: {
          backgroundColor: '#2E3B55' 
        }
      }
    },
    MuiButton: {
        styleOverrides: {
            root: {
                textTransform: 'none', 
            }
        }
    }
  }
});

export default theme;