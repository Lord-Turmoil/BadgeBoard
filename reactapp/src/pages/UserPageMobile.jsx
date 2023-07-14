import { useEffect, useState } from "react";

import { useNavigate, useParams } from "react-router-dom";

import { Avatar } from "@mui/material";

import User from "../components/user/User";
import InflateBox from "../components/layout/inflate";
import ExpandFab from "../components/utility/ExpandFab";
import { useLocalUser, useUser } from "../components/user/useUser";

import '../assets/css/pages/user/user.mobile.css'


/*
Parent could not get states of child component, but can use callback
to update their copy in parent.
*/
export default function UserPageMobile() {
    const navigate = useNavigate();

    const { uid } = useParams('uid');

    // current logged in user
    const {
        data: visitor
    } = useLocalUser();

    // current visiting user
    const {
        data: user,
        loading: userLoading,
        error: userError
    } = useUser(uid ? uid : (visitor ? visitor.account.id : null), () => { navigate("/404"); });

    const isOwner = () => {
        if ((visitor == null) || (user == null)) {
            return false;
        }
        return visitor.account.id == user.account.id;
    }

    // expand toggle 
    const [expandOn, setExpandOn] = useState(false);
    const toggleExpand = () => {
        setExpandOn(!expandOn);
    }

    // data format
    const formatBirthday = (u) => {
        var birthday = User.birthday(u);
        if ((birthday == null) || (birthday == "")) {
            return null;
        }
        return birthday;
    }

    return (
        <div className="user-mobile-main">
            <div className="nav-wrapper">
                <ExpandFab onClick={toggleExpand} />
                <div className="nav"></div>
            </div>
            <div className={`panel${expandOn ? " active" : ""}`}>
                <div className="primary">
                    <div className="avatar">
                        <Avatar sx={{ width: 100, height: 100 }}>TS</Avatar>
                    </div>
                    <div className="info-wrapper">
                        <h2 className="username">{User.username(user)}</h2>
                        <div className="motto"><p>{User.motto(user)}</p></div>
                    </div>
                </div>
                <div className="username"></div>
                <div className="motto"></div>
                {isOwner() ? <div className="edit"></div> : null}
            </div>
            <InflateBox sx={{ backgroundColor: 'lightBlue' }} overflow>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
                <h1>Hello</h1>
            </InflateBox>
        </div>
    );
}
