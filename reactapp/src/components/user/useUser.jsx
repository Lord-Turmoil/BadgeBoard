import { useEffect, useState } from "react";
import api from "../api";

const useUser = (uid) => {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        let didCancel = false;
        
        setError(null);
        if (uid) {
            (async () => {
                try {
                    setLoading(true);
                    var dto = await api.get("user/user", { id: uid });
                    if (dto.meta.status != 0) {
                        throw new Error(dto.meta.message);
                    }
                    setData(dto.data);
                } catch (err) {
                    setError(err);
                } finally {
                    setLoading(false);
                }
            })();
        } else {
            setData(null);
            setLoading(false);
        }
        return () => {
            didCancel = true;
        }
    }, [uid]);

    return { data, loading, error };
}

export default useUser;
