import { useCallback, useRef, useState } from "react";
import BadgeContainerMobile from "~/components/layout/BadgeContainer/BadgeContainerMobile";
import NoteContainer from '~/components/display/Note/NoteContainer/NoteContainer';
import QuestionNote from '~/components/display/Note/QuestionNote/QuestionNote';
import { CircularProgress } from "@mui/material";

import './BadgeBoardMobile.css';
import useInterval from "~/services/hook/useInterval";

export default function BadgeBoardMobile({
    badges = null
}) {
    const [loadingText, setLoadingText] = useState();
    const [loadingCount, setLoadingCount] = useState(0);

    useInterval(() => {
        var text = "Loading";
        for (var i = 0; i < loadingCount; i++) {
            text += ".";
        }
        setLoadingText(text);
        setLoadingCount(loadingCount >= 3 ? 0 : loadingCount + 1);
    }, 500);

    return (
        <BadgeContainerMobile className='BadgeBoard BadgeBoardMobile'>
            {badges ?
                <div className="BadgeBoard__board">
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
                </div>
                :
                <div className="BadgeBoard__loading">
                    <CircularProgress size='60%' color="secondary" />
                    <h3>{loadingText}</h3>
                </div>
            }
        </BadgeContainerMobile>
    );
};