import { Routes, Route } from 'react-router-dom'

import HomePage from '~/pages/HomePage'
import RegisterPage from '~/pages/sign/RegisterPage'
import LoginPage from '~/pages/sign/LoginPage'
import UserPage from '~/pages/user/UserPage'
import NotFoundPage from '~/pages/NotFoundPage'
import { Component } from 'react'

export default class App extends Component {
    render() {
        return (
            <Routes>
                <Route exact path="/" element={<HomePage></HomePage>}></Route>
                <Route exact path="/register" element={<RegisterPage></RegisterPage>}></Route>
                <Route exact path="/login" element={<LoginPage></LoginPage>}></Route>
                <Route exact path="/user/:uid" element={<UserPage></UserPage>}></Route>
                <Route exact path="*" element={<NotFoundPage></NotFoundPage>}></Route>
            </Routes>
        );
    }
}
