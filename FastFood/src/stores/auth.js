import { defineStore } from 'pinia';
import axios from 'axios';
import jwtDecode from 'jwt-decode'

// Pinia za spremanje stanja, storage itd.
export const useAuthStore = defineStore('auth', {
state: () => ({
    // Dohvacam token i role iz local storagea
    token: localStorage.getItem('token') || '',
    roles: JSON.parse(localStorage.getItem('roles') || '[]'),
    user: JSON.parse(localStorage.getItem('user') || 'null')
  }),
  actions: {

    // Funckija za registraciju, pravim preko ApplicationUser novog korisnika
     async register(username, email, password, firstName, lastName) {
      await axios.post('/api/auth/register', {
        username,
        email,
        password,
        firstName,
        lastName,
        rola: '22222222-bbbb-bbbb-bbbb-222222222222' 
      })
     },    
    // Funckija prima username i password, dohvaca podate
    async login(username, password) {
      const { data } = await axios.post('/api/auth/login', { username, password });
      // dekodira token da bi mogao dohvatiti role u objekt
      const decoded = jwtDecode(data.token)     
      // gleda rolu korisnika, admin ili user
      const roles   = Array.isArray(decoded.role) ? decoded.role : [decoded.role]
      this.token = data.token;
      this.roles = roles
      this.user = {
      id:        decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'], // Vraća id korisnika preko urla
      email:     decoded.email,
      firstName: decoded.FirstName,
      lastName:  decoded.LastName,
      roles:     Array.isArray(decoded.role) ? decoded.role : [decoded.role]
      };

      // spremam u local storage 
      localStorage.setItem('token', data.token);
      localStorage.setItem('roles',  JSON.stringify(roles))
      axios.defaults.headers.common.Authorization = `Bearer ${data.token}`;
      localStorage.setItem('user', JSON.stringify(this.user))
      console.log('Decoded token:', decoded)
      console.log(jwtDecode(localStorage.getItem('token')))
    },
    logout() {

      // brišem podatke iz local storagea nakon logouta
      this.token = '';
      this.roles = []
      localStorage.removeItem('token');
      localStorage.removeItem('roles')
      localStorage.removeItem('user')
      delete axios.defaults.headers.common.Authorization;

      

    }
    
  }
});
