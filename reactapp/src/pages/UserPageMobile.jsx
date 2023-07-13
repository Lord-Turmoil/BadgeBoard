import { useEffect, useState } from "react";

import { useNavigate, useParams } from "react-router-dom";

import User from '../components/user/user';
import useUser from "../components/user/useUser";

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
            <h1>User Page Mobile {uid}</h1>
            { userError ? <div>{JSON.stringify(userError.message)}</div> : null }
            {userLoading ? <div>Loading...</div> :
                <div>
                    <div>
                        {JSON.stringify(user)}
                    </div>
                    <div>

                        {JSON.stringify(visitor)}
                    </div>
                </div>
            }
        </div>
    );
}
