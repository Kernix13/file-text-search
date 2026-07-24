import { useState, useEffect } from "react";
import SearchForm from "./components/SearchForm";

const API_URL = 'http://localhost:5042/api/search';

const App = () => {
  // const [searchPhrase, setSearchPhrase] = useState("");
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
      <h1>System File Search Using C#</h1>
      {/* { loading ? <p>Loading...</p> : null } */}
      { loading && <p>Loading...</p> }

      { error && <p className="error">{ error }</p> }

      { !loading && !error && (
        <>
          {/* <h2>Search results for "{searchPhrase}"</h2> */}
          <main className="grid">
            { searchResults.map(result => (
              <div key={result.id} className="result-card">
                {/* just a list for now: */}
                <button>Delete</button>
                <ul>
                  <li>{result.category}</li>
                  {/* <li>{result.fileName}</li> */}
                  <li><a className="result-link" target="_blank" href="https://www.google.com/">{result.fileName}</a></li>
                  <li>{result.priority}
                    <button>Edit</button>
                  </li>
                </ul>
              </div>
            )) }
          </main>
        </>
      ) }
      <SearchForm />
    </div>
   );
}
 
export default App;