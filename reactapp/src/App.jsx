import { Component } from 'react'

import { Route, Routes } from 'react-router-dom'

import HomePage from '~/pages/HomePage'
import UserPage from '~/pages/user/UserPage'
import LoginPage from '~/pages/sign/LoginPage'
import NotFoundPage from '~/pages/NotFoundPage'
import RegisterPage from '~/pages/sign/RegisterPage'

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