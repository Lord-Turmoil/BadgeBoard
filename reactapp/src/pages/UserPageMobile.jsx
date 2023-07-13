import { useEffect, useState } from "react";

import { useNavigate, useParams } from "react-router-dom";

import User from '../components/user/user';
import useUser from "../components/user/useUser";
import InflateBox from "../components/layout/inflate";

export default function UserPageMobile() {
    const navigate = useNavigate();

    const { uid } = useParams('uid');

    // current logged in user
    const [visitor, setVisitor] = useState(User.get());

    // current visiting user
    const {
        data: user,
        loading: userLoading,
        error: userError
    } = useUser(uid ? uid : (visitor ? visitor.account.id : null));

    return (
        <div>
            <InflateBox sx={{backgroundColor: 'red'}} overflow>
                <h1>User Page Mobile</h1>
            </InflateBox>
        </div>
    );
}
