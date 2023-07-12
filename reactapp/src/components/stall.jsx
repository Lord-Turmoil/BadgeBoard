export function delay(ms) {
    return new Promise((resolve) => {
        setTimeout(() => { resolve() }, ms);
    });
}

// make task complete at least 'ms' ms later
export default async function stall(task, ms) {
    var [ret, _] = await Promise.all([task, delay(ms)]);
    return ret;
}