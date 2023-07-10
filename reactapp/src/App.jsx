import { Routes, Route } from 'react-router-dom'

import HomePage from './pages/HomePage'
import UserPage from './pages/UserPage'
import NotFoundPage from './pages/NotFoundPage'

function App() {
    return (
        <div>
            <Routes>
                <Route exact path="/" element={<HomePage></HomePage>}></Route>
                <Route exact path="/user/:uid" element={<UserPage></UserPage>}></Route>
                <Route exact path="*" element={<NotFoundPage></NotFoundPage>}></Route>
            </Routes>
        </div>
    );
}

export default App;
