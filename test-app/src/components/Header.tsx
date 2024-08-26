import {useState} from "react";


export default function Header(props: {label: string}) {

  const [now, setNow] = useState(new Date())
  setInterval(() => setNow(new Date()), 1000)
  return (
    <header>
      <span>{props.label}</span><br/>
      <span>time is: {now.toLocaleTimeString()}</span>
    </header>
  )
}