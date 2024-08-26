import axios from "axios";

const URL='https://localhost:7289/api/Chess'

const addMatch = async (match) => {
    try {
        const response = await axios.post(URL, match);
        return { data: response.data, loading: false, error: null };
    } catch (error) {
        return { data: null, loading: false, error: error };
    }
}

const GetPlayerByCountry=async (country)=>{
    let url=`${URL}/PlayerByCountry`
    try {
        const response = await axios.get(url,{
            params: {
                country: country
            }
        });
        return { data: response.data, loading: false, error: null };
    } catch (error) {
        return { data: null, loading: false, error: error };
    }
}

const GetPlayerPerformance=async ()=>{
    let url=`${URL}/PlayerPerformance`
    try {
        const response = await axios.get(url);
        return { data: response.data, loading: false, error: null };
    } catch (error) {
        return { data: null, loading: false, error: error };
    }
}

const GetHighestWonPlayer = async () => {
    let url=`${URL}/PlayerWithHigeshtWin`
    try {
        const response = await axios.get(url);
        console.log(response.data)
        return { data: response.data, loading: false, error: null };
    } catch (error) {
        return { data: null, loading: false, error: error };
    }
};

export {addMatch, GetPlayerByCountry, GetPlayerPerformance, GetHighestWonPlayer}