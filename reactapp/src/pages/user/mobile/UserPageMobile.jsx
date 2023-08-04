import { useEffect, useRef, useState } from 'react';

import { useNavigate, useParams } from 'react-router-dom';

import notifier from '~/services/notifier';
import ExpandFab from '~/components/utility/ExpandFab';
import InflateBox from '~/components/layout/InflateBox';
import { fetchUser, useLocalUser, useUser } from '~/services/user/UserUtil';

import UserInfoPanel from '~/parts/UserPanel/UserInfoPanel/UserInfoPanel';

import '~/parts/UserPanel/UserPanel.css'
import './UserPageMobile.css'
import UserBasicNav from '~/parts/UserPanel/UserNav/UserNav';
import _debounce from 'debounce';
import { fetchCategories } from '~/services/user/CategoryUtil';
import CategorySelect from '~/parts/UserPanel/CategorySelect/CategorySelect';
import stall from '~/services/stall';
import NoteContainer from '~/components/display/Note/NoteContainer/NoteContainer';
import BadgeContainerMobile from '~/components/layout/BadgeContainer/BadgeContainerMobile';
import UserPanelPadding from '~/parts/UserPanel/UserPanelPadding';
import QuestionNote from '~/components/display/Note/QuestionNote/QuestionNote';

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
    const expandExclude = useRef(null);
    const toggleExpand = () => {
        setExpandOn(!expandOn);
    };

    // user category
    const [categories, setCategories] = useState(null);
    const [categoryError, setCategoryError] = useState(null);
    const [currentCategory, setCurrentCategory] = useState(null);
    const [currentCategoryIndex, setCurrentCategoryIndex] = useState(null);
    useEffect(() => {
        if (categories && (currentCategoryIndex != null)) {
            setCurrentCategory(categories[currentCategoryIndex]);
        }
    }, [currentCategoryIndex]);

    useEffect(() => {
        console.log("🚀 > useEffect > currentCategory:", currentCategory);
    }, [currentCategory]);

    useEffect(() => {
        if (user) {
            (async () => {
                const [c, d, e] = await stall(
                    fetchCategories(user.account.id, visitor ? visitor.account.id : null), 3000);
                if (e) {
                    notifier.error(e);
                    setCategoryError(e);
                } else {
                    setCategories(c);
                    setCurrentCategoryIndex(d);
                }
            })();
        }
    }, [user]);

    return (
        <div className="UserPanel UserPanel__mobile">
            <div className="nav-wrapper">
                <ExpandFab ref={expandExclude} open={expandOn} setOpen={setExpandOn} />
                <CategorySelect
                    categories={categories}
                    currentCategory={currentCategory}
                    setCategoryIndex={setCurrentCategoryIndex} />
                <UserBasicNav user={visitor} />
            </div>
            <UserInfoPanel
                user={user}
                visitor={visitor}
                onUserChange={onUserChange}
                onVisitorChange={onVisitorChange}
                disabled={!expandOn}
                onClose={() => setExpandOn(false)}
                exclude={expandExclude.current} />
            <InflateBox sx={{ backgroundColor: 'azure' }} overflow>
                <UserPanelPadding />
                <BadgeContainerMobile>
                    <NoteContainer rotate={8}>
                        <QuestionNote question='This is a short question' />
                    </NoteContainer>
                    <NoteContainer rotate={8} variant='style-1'>
                        <QuestionNote
                            question='This is a long long long long long long long long long long long long question'
                            answer='This is a good good good good good question' />
                    </NoteContainer>
                    <NoteContainer rotate={8} variant='style-2'>
                        <QuestionNote
                            question='你有没有喜欢的人呀(❤ ω ❤)'
                            answer='没有捏' />
                    </NoteContainer>
                    <NoteContainer rotate={8} variant='style-3'>
                        <QuestionNote
                            question='你有没有喜欢的电影呀(❤ ω ❤)'
                            answer='当然有啦，我最喜欢《星球大战》了！' />
                    </NoteContainer>
                    <NoteContainer rotate={8} variant='style-4'>
                        <QuestionNote
                            question='你有没有喜欢的语言呀(❤ ω ❤)'
                            answer='当然有啦，我最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最喜欢 C++ 了' />
                    </NoteContainer>
                    <NoteContainer rotate={8} variant='style-5'>
                    <QuestionNote
                            question='你有没有喜欢的语言呀(❤ ω ❤)'
                            answer='当然有啦，我最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最喜欢 C++ 了' />
                    </NoteContainer>
                    <NoteContainer rotate={8} variant='style-6'>
                    <QuestionNote
                            question='你有没有喜欢的语言呀(❤ ω ❤)'
                            answer='当然有啦，我最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最最喜欢 C++ 了' />
                    </NoteContainer>
                    <NoteContainer rotate={8} />
                    <NoteContainer rotate={8} />
                </BadgeContainerMobile>
            </InflateBox>
        </div>
    );
}