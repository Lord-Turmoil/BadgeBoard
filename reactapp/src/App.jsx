import { Routes, Route } from 'react-router-dom'

import HomePage from './pages/HomePage'
import RegisterPage from './pages/RegisterPage'
import LoginPage from './pages/LoginPage'
import UserPage from './pages/UserPage'
import NotFoundPage from './pages/NotFoundPage'
import NavBarDev from './parts/NavBarDev'

function App() {
    return (
        <div>
            <NavBarDev></NavBarDev>
            <Routes>
                <Route exact path="/" element={<HomePage></HomePage>}></Route>
                <Route exact path="/register" element={<RegisterPage></RegisterPage>}></Route>
                <Route exact path="/login" element={<LoginPage></LoginPage>}></Route>
                <Route exact path="/user/:uid" element={<UserPage></UserPage>}></Route>
                <Route exact path="*" element={<NotFoundPage></NotFoundPage>}></Route>
            </Routes>
        </div>
    );
}

export default App;
