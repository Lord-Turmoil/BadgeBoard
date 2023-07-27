import { useCallback, useEffect, useState } from 'react';

import { Avatar } from '@mui/material';
import CameraAltRoundedIcon from '@mui/icons-material/CameraAltRounded';

import notifier from '~/services/notifier';
import AvatarUtil from '~/services/user/AvatarUtil';
import ImageUtil from '~/services/image/ImageUtil'
import PopupModal from '~/components/layout/PopupModal';
import ImageCrop from '~/components/utility/ImageCrop/ImageCrop';

import './AvatarField.css';
import api from '~/services/api';

export default function AvatarField({
    size = 100,
    src,
    onAvatarChange = null,
    disabled = true
}) {
    // propertied
    const [showDialog, setShowDialog] = useState(false);

    const [imageData, setImageData] = useState(src);
    const [oldImageData, setOldImageData] = useState(null);

    // file upload
    const onClickUpload = (event) => {
        const input = document.createElement('input');
        input.type = 'file';
        input.accept = 'image/*';
        input.addEventListener('change', handleFileSelection);
        input.click();
    };

    const handleFileSelection = async (event) => {
        event.preventDefault();
        try {
            let file;
            if (event.dataTransfer) {
                file = event.dataTransfer.files[0];
            } else if (event.target) {
                file = event.target.files[0];
            }
            if (file.size / 1024 / 1024 > 3.0) {
                notifier.error('Avatar should be less than 3 MB');
                return;
            }
            const reader = new FileReader();
            if (!reader) {
                return;
            }
            reader.onload = () => {
                setOldImageData(imageData);
                setImageData(reader.result?.toString());
                setShowDialog(true);
            };
            reader.readAsDataURL(file);
        } catch (error) {
            console.log('🚀 > handleFileSelect > error:', error);
        }
    };

    // cropped area (in pixel integer)
    const [croppedArea, setCroppedArea] = useState('');

    const getCroppedImage = useCallback(async () => {
            try {
                return await ImageUtil.getCroppedImg(imageData, croppedArea, 0);
            } catch (error) {
                notifier.error(error.message);
                console.error('🚀 > getCroppedImage > error:', error);
                return null;
            }
        },
        [croppedArea, imageData]);

    // cancellation and confirmation
    const onCancelSelection = () => {
        setImageData(oldImageData ?? src);
        setShowDialog(false);
    };

    const onConfirmSelection = async () => {
        setShowDialog(false);

        const data = await getCroppedImage();
        if (data == null) {
            notifier.error('Failed to crop avatar');
            return;
        }
        setImageData(data);
        var avatarUrl = await uploadAvatar(data);
        if (avatarUrl) {
            onAvatarChange && onAvatarChange(avatarUrl);
        }
    };

    const uploadAvatar = async (data) => {
        var dto = await api.post('user/avatar',
            {
                extension: 'jpeg',
                data: data
            });
        notifier.auto(dto.meta);

        if (dto.meta.status != 0) {
            onCancelSelection();
            return null;
        }

        return dto.data;
    };

    return (
        <div className="AvatarField">
            <Avatar sx={{ width: size, height: size }} src={AvatarUtil.getUrl(imageData)} alt="Avatar"/>
            <div className={`AvatarField__mask${disabled ? '' : ' AvatarField__active'}`}>
                <div className="AvatarField__upload" onClick={disabled ? null : onClickUpload}>
                    <CameraAltRoundedIcon/>
                </div>
            </div>
            <PopupModal
                open={showDialog}
                title="Edit Avatar"
                onClose={onCancelSelection}
                onCancel={onCancelSelection}
                onConfirm={onConfirmSelection}>
                <ImageCrop
                    image={AvatarUtil.getUrl(imageData)}
                    croppedArea={croppedArea}
                    onCroppedAreaChange={setCroppedArea}/>
            </PopupModal>
        </div>
    );
};