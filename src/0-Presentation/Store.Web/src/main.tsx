import { StrictMode } from 'react'
import { Provider } from 'react-redux'
import { store} from '@/store'
import { createRoot } from 'react-dom/client'
import { RouterProvider } from 'react-router-dom'
import { router } from '@/features/routes'
import { setUser } from '@/features/auth/authSlice'
import keycloak from './services/keycloak'
import 'bootstrap/dist/css/bootstrap.min.css'

keycloak.init({onLoad: 'check-sso', pkceMethod: 'S256', checkLoginIframe:false, enableLogging:true}).then((authenticated) => {
  if(authenticated) {
    console.log(`keycloak`, keycloak)
    store.dispatch(setUser(
      {
        username: keycloak.tokenParsed?.preferred_username || '',
        given_name: keycloak.tokenParsed?.given_name || '',
        email: keycloak.tokenParsed?.email || '',
        roles: keycloak.tokenParsed?.roles || [],
        token: keycloak.token || ''
      }))
  }
  createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <Provider store={store}>
      <RouterProvider router={router}/>
    </Provider>
  </StrictMode>,
)
});
