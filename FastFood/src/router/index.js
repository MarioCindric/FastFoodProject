import { createRouter, createWebHistory, } from 'vue-router'
import Pocetna from '../views/Pocetna.vue'
import Login from '../views/Login.vue'
import Registracija from '../views/Registracija.vue'
import Narudzba from '../views/Narudzba.vue'
import PocetnaUser from '../views/PocetnaUser.vue'
import MojProfilAdmin from '../views/admin/MojProfilAdmin.vue'
import Statistika from '../views/admin/Statistika.vue'
import PovijestNarudzbi from '../views/user/PovijestNarudzbi.vue'
import MojProfilUser from '../views/user/MojProfilUser.vue'
import NarudzbeZaposlenik from '../views/Employee/NarudzbeZaposlenik.vue'
import ObradaNarudzbe from '../views/Employee/ObradaNarudzbe.vue'
import ZavrseneNarudzbe from '../views/Employee/ZavrseneNarudzbe.vue'
import NarudzbeDostava from '../views/Employee/NarudzbeDostava.vue'
import AktivneNarudzbe from '../views/user/AktivneNarudzbe.vue'
import PotvrdaNarudzbe from '../views/PotvrdaNarudzbe.vue'
import NotFound from '../views/NotFound.vue'
import Zaposlenici from '../views/admin/Zaposlenici.vue'
import AdministracijaJela from '../views/Employee/AdministracijaJela.vue'
import JeloEdit from '../views/Employee/JeloEdit.vue'
import DodajJelo from '../views/Employee/DodajJelo.vue'
import OtkazaneNarudzbe from '../views/user/OtkazaneNarudzbe.vue'
import Korisnici from '../views/admin/Korisnici.vue'

import { useAuthStore } from '../stores/auth'
import PracenjeNarudzbi from '../views/Employee/PracenjeNarudzbi.vue'


const routes = 
[
    {
        path:"/",
        name:"Pocetna",
        component:Pocetna
    },

    {
        path:"/Login",
        name:"Login",
        component:Login
    },
        
    {
        path:"/Registracija",
        name:"Registracija",
        component:Registracija
    },

    {
        path:"/narudzba",
        name:"Naruzba",
        component:Narudzba
    },

    {
        path:"/Pocetna",
        name:"PocetnaUser",
        component:PocetnaUser
    },

    {
        path:"/Profil",
        name:"Profil",
        component:MojProfilAdmin
    },

    {
        path:"/Statistika",
        name:"Statistika",
        component:Statistika
    },

    {
        path:"/KorisnickiProfil",
        name:"KorisnickiProfil",
        component:MojProfilUser
    },

    {
        path:"/Povijest",
        name:"Povijest",
        component:PovijestNarudzbi
    },
    {
        path:"/KreiraneNarudzbe",
        name:"KreiraneNarudzbe",
        component:NarudzbeZaposlenik
    },

    {
        path:"/ObradaNarudzbe",
        name:"ObradaNarudzbe",
        component:ObradaNarudzbe
    },
    
    {
        path:"/ZavrseneNarudzbe",
        name:"ZavrseneNarudzbe",
        component:ZavrseneNarudzbe
    },

    {
        path: "/NarudzbeDostava",
        name: "NarudzbeDostava",
        component: NarudzbeDostava
    },

    {
        path:"/AktivneNarudzbe",
        name:"AktivneNarudzbe",
        component:AktivneNarudzbe
    },
    {
        path:"/PotvrdaNarudzbe",
        name:"PotvrdaNarudzbe",
        component:PotvrdaNarudzbe
    },

    {
        path:"/404",
        name:"NotFound",
        component:NotFound
    },

    {
        path:"/Zaposlenici",
        name:"Zaposlenici",
        component:Zaposlenici
    },

    {
        path:"/AdministracijaJela",
        name:"AdministracijaJela",
        component:AdministracijaJela
    },

    {
        path:"/jelo/:id",
        name:"jelo",
        component:JeloEdit
    },

    {
        path: "/DodajJelo",
        name: "DodajJelo",
        component: DodajJelo
    },
    {
        path: "/OtkazaneNarudzbe",
        name: "OtkazaneNarudzbe",
        component: OtkazaneNarudzbe
    },
    {
        path:"/Korisnici",
        name:"Korisnici",
        component:Korisnici
    },

    {
        path:"/PracenjeNarudzbi",
        name:"PracenjeNarudzbi",
        component: PracenjeNarudzbi
    }
    

]

const router = createRouter({
    history: createWebHistory(),
    routes
})

router.beforeEach((to, from, next) => {
  const auth = useAuthStore()
  const roles = auth.roles || []

  const adminRoutes = [
    '/Statistika',
    '/Zaposlenici',
    '/Korisnici'
  ]

  const zaposlenikRoutes = [
    '/KreiraneNarudzbe',
    '/ObradaNarudzbe',
    '/NarudzbeDostava',
    '/PracenjeNarudzbi',

  ]

  const userRoutes = [
    '/Pocetna',
    '/KorisnickiProfil',
    '/AktivneNarudzbe',
    '/OtkazaneNarudzbe',
    '/Povijest'
  ]

  // zajednička ruta za admina i zaposlenika
  if (
  (to.path === '/ZavrseneNarudzbe' || 
   to.path === '/AdministracijaJela' || 
   to.path === '/DodajJelo' || 
   to.path === '/jelo/:id') &&
  !(roles.includes('Admin') || roles.includes('Zaposlenik'))
) {
  return next('/404')
}


  if (adminRoutes.includes(to.path) && !roles.includes('Admin')) {
    return next('/404')
  }

  if (zaposlenikRoutes.includes(to.path) && !roles.includes('Zaposlenik')) {
    return next('/404')
  }

  if (userRoutes.includes(to.path) && !roles.includes('User')) {
    return next('/404')
  }

  next()
})

export default router;