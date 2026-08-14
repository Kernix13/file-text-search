import { useState, useEffect } from "react";
import SearchForm from "./components/SearchForm";

const API_URL = 'http://localhost:5042/api/search';

const App = () => {
  const [searchPhrase, setSearchPhrase] = useState("");
  const [fileType, setFileType] = useState("");
  const [folder, setFolder] = useState("");
  const [searchResults, setSearchResults] = useState([]);
  
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log(searchPhrase, fileType, folder);
    // call the API here

    // I need to clear the search form when done
  }

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
      <h1>System File Search</h1>
      {/* I need this to show the search phrase AFTER the form has been submitted */}
      <h2>Search results for: '{searchPhrase}'</h2>
      <form className="search-form" onSubmit={handleSubmit}>

        <div className="form-control">
          <label htmlFor="filesearch">Search your files:</label>
          <input
            type="search"
            id="filesearch"
            value={searchPhrase}
            onChange={e => setSearchPhrase(e.target.value)}
            placeholder="Enter search phrase"
          />
        </div>

        <div className="form-control">
          <label htmlFor="type">Show: </label>
          <select value={fileType} id="type" onChange={(e) => setFileType(e.target.value)}>
            <option value="md">md</option>
            <option value="txt">txt</option>
            <option value="css">css</option>
            <option value="html">html</option>
            <option value="js">js</option>
          </select>
        </div>

        <div className="form-control">
          <label htmlFor="folder">Enter folder to search:</label>
          <input
            type="text"
            id="folder"
            value={folder}
            onChange={e => setFolder(e.target.value)}
            placeholder="Folder path (optional)"
          />
        </div>

        <button className="form-btn" type="submit">Search</button>
      </form>

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
                <button className="delete-btn">Delete</button>
                <ul>
                  <li>{result.category}</li>
                  {/* <li>{result.fileName}</li> */}
                  <li><a 
                    className="result-link" 
                    target="_blank" 
                    href={`file:///${result.fullPath.replace(/\\/g, "/")}`}
                  >{result.fileName}</a></li>
                  <li>File path: {`file:///${result.fullPath.replace(/\\/g, "/")}`}</li>
                  <li><button className="copy-btn">Copy file path</button></li>
                  <li>Priority: {result.priority}
                    <button className="edit-btn">Edit</button>
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