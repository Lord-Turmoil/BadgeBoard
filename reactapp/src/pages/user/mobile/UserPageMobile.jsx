import { useEffect, useState } from 'react';

import { useNavigate, useParams } from 'react-router-dom';

import notifier from '~/services/notifier';
import ExpandFab from '~/components/utility/ExpandFab';
import InflateBox from '~/components/layout/InflateBox';
import { fetchUser, useLocalUser, useUser } from '~/services/user/UserUtil';

import UserInfoPanel from '~/parts/UserPanel/UserInfoPanel/UserInfoPanel';

import '~/parts/UserPanel/UserPanel.css'
import './UserPageMobile.css'
import UserBasicNav from '~/parts/UserPanel/UserNav/UserBasicNav/UserBasicNav';
import _debounce from 'debounce';
import { fetchCategories } from '~/services/user/CategoryUtil';
import UserAdvancedNav from '~/parts/UserPanel/UserNav/UserAdvancedNav/UserAdvancedNav';
import CategorySelect from '~/parts/UserPanel/CategorySelect/CategorySelect';

/*
Parent could not get states of child component, but can use callback
to update their copy in parent.
*/
export default function UserPageMobile() {
    const navigate = useNavigate();
    const { uid } = useParams('uid');
    const [update, setUpdate] = useState("static");

    // current logged in user
    const {
        data: visitor,
        setData: setVisitor,
        loading: visitorLoading,
        error: visitorError
    } = useLocalUser(update);

    useEffect(() => {
        console.log('🚀 > useEffect > visitor:', visitor);
    }, [visitor]);

    // current visiting user
    const [user, setUser] = useState(null);
    const [userError, setUserError] = useState(null);

    useEffect(() => {
        if (visitorLoading || visitor) {
            return;
        }
        console.log("🚀 > useEffect > visitorLoading:", visitorLoading);
        (async () => {
            const [u, e] = await fetchUser(uid);
            if (e) {
                setUserError(e);
            } else {
                setUser(u);
            }
        })();
    }, [visitorLoading]);
    useEffect(() => {
        console.log('🚀 > useEffect > user:', user);
    }, [user]);

    // user error handling
    useEffect(() => {
        if (userError) {
            notifier.error(userError.message);
            setTimeout(() => { navigate('/404') }, 0);
        }
    }, [userError]);

    // user change handling
    const onUserChange = (data) => {
        setUser({ ...user, [data.key]: data.value });
    };
    const onVisitorChange = (data) => {
        setVisitor({ ...visitor, [data.key]: data.value });
    };

    // expand toggle
    const [expandOn, setExpandOn] = useState(false);
    const toggleExpand = () => {
        setExpandOn(!expandOn);
    };

    // user category
    const [categories, setCategories] = useState(null);
    const [categoryError, setCategoryError] = useState(null);
    const [currentCategory, setCurrentCategory] = useState(null);
    useEffect(() => {
        if (!user) {
            return;
        }
        (async () => {
            const [c, d, e] = await fetchCategories(uid, visitor ? visitor.account.id : null);
            if (e) {
                notifier.error(e);
                setCategoryError(e);
            } else {
                setCategories(c);
                setCurrentCategory(d);
                console.log("🚀 > useEffect > c:", c);
            }
        })();
    }, [user]);

    return (
        <div className="UserPanel UserPanel__mobile">
            <div className="nav-wrapper">
                <ExpandFab open={expandOn} setOpen={setExpandOn} />
                <div className="nav">
                    <UserAdvancedNav user={visitor}>
                        <CategorySelect
                            categories={categories}
                            currentCategory={currentCategory}
                            setCategory={setCurrentCategory} />
                    </UserAdvancedNav>
                </div>
            </div>
            <UserInfoPanel
                user={user}
                visitor={visitor}
                onUserChange={onUserChange}
                onVisitorChange={onVisitorChange}
                disabled={!expandOn}
                onClose={null} />
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