import { useState, useEffect } from "react";
import SearchForm from "./components/SearchForm";

const API_URL = 'http://localhost:5042/api/search';

const App = () => {
  const [searchResults, setSearchResults] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchResults = async () => {
      try {
        const response = await fetch(API_URL);
        if (!response.ok) throw new Error('Failed to fetch data'); 
        const data = await response.json();
        console.log(data);
        setSearchResults(data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    }
    fetchResults();
  }, []);

  return ( 
    <div>
      <SearchForm />
    </div>
   );
}
 
export default App;