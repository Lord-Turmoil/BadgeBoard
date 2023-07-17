import { useEffect, useState } from 'react';

import { useNavigate, useParams } from 'react-router-dom';

import notifier from '~/services/notifier';
import ExpandFab from '~/components/utility/ExpandFab';
import InflateBox from '~/components/layout/InflateBox';
import { useLocalUser, useUser } from '~/services/user/UserUtil';

import UserInfoPanel from '~/parts/UserPanel/UserInfoPanel/UserInfoPanel';

import '~/parts/UserPanel/UserPanel.css'
import './UserPageMobile.css'
import UserBasicNav from '~/parts/UserPanel/UserNav/UserBasicNav/UserBasicNav';

/*
Parent could not get states of child component, but can use callback
to update their copy in parent.
*/
export default function UserPageMobile() {
    const navigate = useNavigate();
    const { uid } = useParams('uid');

    // current logged in user
    const {
        data: visitor,
        setData: setVisitor,
        loading: visitorLoading,
        error: visitorError
    } = useLocalUser();

    useEffect(() => {
        console.log("🚀 > useEffect > visitor:", visitor);
    }, [visitor]);

    // current visiting user
    const {
        data: user,
        setData: setUser,
        loading: userLoading,
        error: userError
    } = useUser(uid ? uid : (visitor ? visitor.account.id : null));

    useEffect(() => {
        console.log("🚀 > useEffect > user:", user);
    }, [user]);

    // user error handling
    useEffect(() => {
        if (userError) {
            notifier.error(userError.message);
            setTimeout(() => { navigate("/404") }, 0);
        }
    }, [userError]);

    // user change handling
    const onUserChange = (data) => {
        setUser({...user, [data.key]: data.data});
    }
    const onVisitorChange = (data) => {
        setVisitor({...visitor, [data.key]: data.data});
    }

    // expand toggle
    const [expandOn, setExpandOn] = useState(false);
    const toggleExpand = () => {
        setExpandOn(!expandOn);
    }

    return (
        <div className="UserPanel UserPanel__mobile">
            <div className="nav-wrapper">
                <ExpandFab onClick={toggleExpand} />
                <div className="nav">
                    <UserBasicNav user={visitor}/>
                </div>
            </div>
            <UserInfoPanel
                user={user}
                visitor={visitor}
                onUserChange={onUserChange}
                onVisitorChange={onVisitorChange}
                disabled={!expandOn}
                onClose={() => { setExpandOn(false) }}
            />
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
