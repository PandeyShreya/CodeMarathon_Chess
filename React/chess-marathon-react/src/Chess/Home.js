import chess from "../Image/chess.jpg"
import Navbar from "./Navbar"

const Home=()=>{
    return <>  
    <Navbar/>
    <img src={chess} style={{ width: "70%"}} alt='IPL Image' />   
    </>
}

export default Home