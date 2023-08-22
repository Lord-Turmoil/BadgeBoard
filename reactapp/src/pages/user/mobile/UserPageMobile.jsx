import { useEffect, useRef, useState } from 'react';

import { useNavigate, useParams, useSearchParams } from 'react-router-dom';

import notifier from '~/services/notifier';
import ExpandFab from '~/components/utility/ExpandFab';
import InflateBox from '~/components/layout/InflateBox';
import { fetchUser, useLocalUser, useUser } from '~/services/user/UserUtil';

import UserInfoPanel from '~/parts/UserPanel/UserInfoPanel/UserInfoPanel';

import '~/parts/UserPanel/UserPanel.css'
import './UserPageMobile.css'
import UserBasicNav from '~/parts/UserPanel/UserNav/UserNav';
import _debounce from 'debounce';
import { fetchCategories } from '~/services/badge/CategoryUtil';
import CategorySelect from '~/parts/UserPanel/CategorySelect/CategorySelect';
import stall from '~/services/stall';
import UserPanelPadding from '~/parts/UserPanel/UserPanelPadding';
import BadgeBoardMobile from '~/parts/BadgeBoard/BadgeBoardMobile/BadgeBoardMobile';
import { getBadges } from '~/services/badge/BadgeUtil';
import BadgeActionDial from '~/parts/BadgeAction/BadgeActionDial';
import { getActions } from '~/services/action/ActionUtil';
import NoteModal from '~/components/display/BadgeNote/NoteModal/NoteModal';

/*
Parent could not get states of child component, but can use callback
to update their copy in parent.
*/
export default function UserPageMobile() {
    const navigate = useNavigate();
    const { uid } = useParams('uid');
    const [update, setUpdate] = useState("static");
    const [searchParams, setSearchParams] = useSearchParams('category');

    const initCategoryId = searchParams.get('category');

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
            setSearchParams({ ...searchParams, 'category': categories[currentCategoryIndex].id });
        }
    }, [currentCategoryIndex]);

    useEffect(() => {
        console.log("🚀 > useEffect > currentCategory:", currentCategory);
    }, [currentCategory]);

    useEffect(() => {
        if (user) {
            (async () => {
                const [c, d, e] = await stall(fetchCategories(user.account.id, visitor ? visitor.account.id : null));
                if (e) {
                    notifier.error(e);
                    setCategoryError(e);
                } else {
                    setCategories(c);
                    var index = d;
                    if (initCategoryId) {
                        for (var i = 0; i < c.length; i++) {
                            if (c[i].id == initCategoryId) {
                                index = i;
                                break;
                            }
                        }
                    }
                    setCurrentCategoryIndex(index);
                }
            })();
        }
    }, [user]);

    // badges
    const [badges, setBadges] = useState(null);
    const [currentBadge, setCurrentBadge] = useState(null);
    const [badgeError, setBadgeError] = useState(null);

    useEffect(() => {
        if (!currentCategory) {
            return;
        }
        var timestamp = null;
        if (badges) {
            timestamp = badges.timestamp;
        }
        (async () => {
            var [data, error] = await getBadges(currentCategory, timestamp, visitor);
            if (error) {
                notifier.error(error);
                setBadgeError(error);
                setBadges(null);
            } else {
                setBadges(data);
            }
        })();
    }, [currentCategory]);

    const noteModalOpen = Boolean(currentBadge);

    useEffect(() => {
        console.log("🚀 > useEffect > currentBadge:", currentBadge);
    }, [currentBadge]);

    // actions
    const [addQuestionModalOpen, setAddQuestionModalOpen] = useState(false);
    const [addMemoryModalOpen, setAddMemoryModalOpen] = useState(false);
    const [addCategoryModalOpen, setAddCategoryModalOpen] = useState(false);
    const [editCategoryModalOpen, setEditCategoryModalOpen] = useState(false);

    const actions = getActions(
        currentCategory,
        visitor,
        () => { setAddQuestionModalOpen(true) },
        () => { setAddMemoryModalOpen(true) },
        () => { setAddCategoryModalOpen(true) },
        () => { setEditCategoryModalOpen(true) }
    );

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
                <BadgeBoardMobile badges={badges} onClickBadge={setCurrentBadge} />
            </InflateBox>
            <BadgeActionDial user={user} visitor={visitor} category={currentCategory} actions={actions} />
            <NoteModal
                open={noteModalOpen}
                onClose={() => setCurrentBadge(null)}
                badge={currentBadge}
                categories={categories}
                user={visitor}
            />
        </div>
    );
}