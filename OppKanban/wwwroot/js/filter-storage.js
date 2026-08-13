(function () {
    const databaseName = "opp-kanban";
    const databaseVersion = 1;
    const storeName = "preferences";

    function openDatabase() {
        return new Promise((resolve, reject) => {
            const request = indexedDB.open(databaseName, databaseVersion);
            request.onupgradeneeded = () => request.result.createObjectStore(storeName);
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }

    async function getValue(key) {
        const database = await openDatabase();
        return new Promise((resolve, reject) => {
            const request = database.transaction(storeName, "readonly").objectStore(storeName).get(key);
            request.onsuccess = () => resolve(request.result);
            request.onerror = () => reject(request.error);
        });
    }

    async function setValue(key, value) {
        const database = await openDatabase();
        return new Promise((resolve, reject) => {
            const request = database.transaction(storeName, "readwrite").objectStore(storeName).put(value, key);
            request.onsuccess = () => resolve();
            request.onerror = () => reject(request.error);
        });
    }

    window.oppFilterStorage = {
        getValue,
        setValue,
        async getCloseYears(key) {
            return (await getValue(key)) ?? [];
        },

        async setCloseYears(key, years) {
            return setValue(key, years);
        },
    };
})();