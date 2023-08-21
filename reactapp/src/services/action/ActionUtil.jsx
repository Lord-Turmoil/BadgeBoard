import CategoryRoundedIcon from '@mui/icons-material/CategoryRounded';
import QuestionMarkRoundedIcon from '@mui/icons-material/QuestionMarkRounded';
import EditNoteRoundedIcon from '@mui/icons-material/EditNoteRounded';
import PermDataSettingRoundedIcon from '@mui/icons-material/PermDataSettingRounded';

export const getActions = (category = null, initiator = null) => {
    console.log("🚀 > getActions > category:", category);
    console.log("🚀 > getActions > initiator:", initiator);
    
    if (category == null) {
        return [];
    }

    const isOwner = initiator && (category.user.id == initiator.account.id);
    let option = category.option;
    var actions = [];
    if (option.isPublic || isOwner) {
        if (option.allowQuestion) {
            actions.push({ icon: <QuestionMarkRoundedIcon />, name: 'Add Question' });
        }
        if (option.allowMemory) {
            actions.push({ icon: <EditNoteRoundedIcon />, name: 'Add Memory' });
        }
    }
    // is owner
    if (isOwner) {
        actions.push({ icon: <CategoryRoundedIcon />, name: 'New Category' });
        actions.push({ icon: <PermDataSettingRoundedIcon />, name: 'Edit Category' });
    }

    return actions;
};