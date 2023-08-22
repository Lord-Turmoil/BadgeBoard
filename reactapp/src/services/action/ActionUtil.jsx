import CategoryRoundedIcon from '@mui/icons-material/CategoryRounded';
import QuestionMarkRoundedIcon from '@mui/icons-material/QuestionMarkRounded';
import EditNoteRoundedIcon from '@mui/icons-material/EditNoteRounded';
import PermDataSettingRoundedIcon from '@mui/icons-material/PermDataSettingRounded';

export const getActions = (
    category = null,
    initiator = null,
    onClickAddQuestion = null,
    onClickAddMemory = null,
    onClickNewCategory = null,
    onClickEditCategory = null
) => {
    if (category == null) {
        return [];
    }

    const isOwner = initiator && (category.user.id == initiator.account.id);
    let option = category.option;
    var actions = [];
    if (option.isPublic || isOwner) {
        if (option.allowQuestion) {
            actions.push({
                icon: <QuestionMarkRoundedIcon />,
                name: 'Add Question',
                callback: onClickAddQuestion
            });
        }
        if (option.allowMemory) {
            actions.push({
                icon: <EditNoteRoundedIcon />,
                name: 'Add Memory',
                callback: onClickAddMemory
            });
        }
    }
    // is owner
    if (isOwner) {
        actions.push({
            icon: <CategoryRoundedIcon />,
            name: 'New Category',
            callback: onClickNewCategory
        });
        actions.push({
            icon: <PermDataSettingRoundedIcon />,
            name: 'Edit Category',
            callback: onClickEditCategory
        });
    }

    return actions;
};